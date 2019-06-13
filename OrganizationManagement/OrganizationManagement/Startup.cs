using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrganizationManagement.Common;
using OrganizationManagement.Constant;
using OrganizationManagement.DataAccess;
using OrganizationManagement.EmailService;
using OrganizationManagement.Models;
using OrganizationManagement.Repositories;

namespace OrganizationManagement
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

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "TestKey";
            })
            .AddJwtBearer("TestKey", x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });
            services.AddMvc();
            //Su dung entity framework
            //services.AddDbContext<CRM_MASTERContext>(options => options.UseSqlServer(Configuration.GetSection("ConnectionStrings").GetSection("MASTERConnection").Value.ToString()));
            services.AddTransient<CRM_MASTERContext>();
            services.AddTransient<IOrganizationRepository, OrganizationDA>();
            services.AddTransient<IEmailService, EmailServiceEx>();
            //Su dung httpcontext
            services.AddHttpContextAccessor();
            OrganizationConstant.SQL_CONNECTION = Configuration.GetSection("ConnectionStrings").GetSection("MASTERConnection").Value.ToString();
            CommonFunction.API_URL = Configuration.GetSection("API").GetSection("Url").Value.ToString();
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
