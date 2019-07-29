using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using VOC.Common;
using VOC.Constant;
using VOC.ContextFactory;
using VOC.DataAccess;
using VOC.Models;
using VOC.Repositories;

namespace VOC
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
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Su dung entity framework
            // Services.AddDbContext<CRM_MPContext>(ServiceLifetime.Transient);
            //services.AddTransient<CRM_MPContext>();
            // Add Interface
            services.AddTransient<IcategoryRepository, CategoryDA>();
            services.AddTransient<IVOCProcessRepository, VOCProcessDA>();
            //services.AddTransient<IExportAndImportRepository, ImportExportDA>();
            //Su dung httpcontext
            services.AddHttpContextAccessor();
            //Su dung cache
            services.AddDistributedRedisCache(options =>
            { options.Configuration = "127.0.0.1:6379"; });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            //Use Policy Role User - HaiHM
            services.AddAuthorization(options =>
            {
                options.AddPolicy(VOCConstant.PolicyCanAdd, policy =>
                  policy.RequireRole(VOCConstant.CanAdd));

                options.AddPolicy(VOCConstant.PolicyEdit, policy =>
                  policy.RequireRole(VOCConstant.CanEdit, VOCConstant.CanEditAll));

                options.AddPolicy(VOCConstant.PolicyDelete, policy =>
                  policy.RequireRole(VOCConstant.CanDelete, VOCConstant.CanDeleteAll));

                options.AddPolicy(VOCConstant.PolicyShow, policy =>
                  policy.RequireRole(VOCConstant.CanShow, VOCConstant.CanShowAll));

            });
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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

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
            VOCCommon attributeCommon = new VOCCommon();
            List<TblConnectionConfig> lstConn = new List<TblConnectionConfig>();
            var objConnection = attributeCommon.GetAllConnection();
            if (objConnection.Count > 0)
            {
                for (int i = 0; i < objConnection[0].Count; i++)
                {
                    TblConnectionConfig item = objConnection[0][i] as TblConnectionConfig;
                    connStrs.Add(item.ConnectionKey, item.ConnectionValue);
                    lstConn.Add(item);
                }
            }
            VOCCommon.ListConnectionStrings = lstConn;
            DbContextFactory dbContextFactory = DbContextFactory.getInstance(connStrs);
            //end
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Category}/{action=GetAllCategory}/{id?}");
            });
        }
    }
}
