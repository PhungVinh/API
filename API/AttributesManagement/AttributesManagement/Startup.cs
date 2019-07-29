using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AttributesManagement.Models;
using AttributesManagement.Constant;
using AttributesManagement.ContextFactory;
using AttributesManagement.Repositories;
using AttributesManagement.DataAccess;
using AttributesManagement.Common;

namespace AttributesManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtConfig = Configuration.GetSection("Jwt");
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig["Key"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = jwtConfig["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtConfig["Issuer"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });
            services.AddMvc();
            //Su dung entity framework
            //services.AddDbContext<CRM_MPContext>(ServiceLifetime.Transient);
            services.AddTransient<CRM_MPContext>();
            services.AddTransient<IAttributeRepository, AttributeDA>();
            services.AddTransient<IcategoryRepository, CategoryDA>();
            services.AddTransient<IExportAndImportRepository, ImportExportDA>();
            //Su dung httpcontext
            services.AddHttpContextAccessor();
            //Su dung cache
            services.AddDistributedRedisCache(options =>
            { options.Configuration = "127.0.0.1:6379"; });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //Multiple database
            Dictionary<string, string> connStrs = new Dictionary<string, string>();
            #region Old: Get from appsetting
            //var lstConnection = Configuration.GetSection(AttributeConstant.ConnectionConfig).AsEnumerable().ToList();
            //if (lstConnection != null)
            //{
            //    for (int i = 1; i < lstConnection.Count; i++)
            //    {
            //        connStrs.Add(lstConnection[i].Key.Replace(AttributeConstant.ConnectionConfigReplace, string.Empty), lstConnection[i].Value);
            //    }
            //}
            #endregion
            AttributeCommon attributeCommon = new AttributeCommon();
            List<ConnectionStrings> lstConn = new List<ConnectionStrings>();
            var objConnection = attributeCommon.GetAllConnection();
            if (objConnection.Count > 0)
            {
                for (int i = 0; i < objConnection[0].Count; i++)
                {
                    ConnectionStrings item = objConnection[0][i] as ConnectionStrings;
                    connStrs.Add(item.ConnectionKey, item.ConnectionValue);
                    lstConn.Add(item);
                }
            }
            AttributeCommon.ListConnectionStrings = lstConn;
            DbContextFactory dbContextFactory = DbContextFactory.getInstance(connStrs);
            //end
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Attribute}/{action=GetOrganizationList}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Attribute", action = "GetOrganizationList" });
            });
        }
    }
}
