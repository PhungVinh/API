using AccountManagement.Constant;
using AccountManagement.DataAccess;
using AccountManagement.EmailService;
using AccountManagement.Models;
using AccountManagement.Repositories;
//using AccountManagement.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace AccountManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /*public void StartExecute()
        {
            EmailConstants.IsRunningEmailExecute = true;
            //QueueingTripsControl<int> queueing = new QueueingTripsControl<int>(int.MaxValue);
            //queueing.EnMyqueue(1);
        }*/

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //JWT authecation
            services.Configure<Controllers.Jwt>(Configuration.GetSection(AccountConstant.strJWT));
            var jwtConfig = Configuration.GetSection(AccountConstant.strJWT);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig["Key"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = jwtConfig[AccountConstant.Issuer],
                ValidateAudience = true,
                ValidAudience = jwtConfig[AccountConstant.Issuer],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(o =>
            {
                //o.DefaultAuthenticateScheme = "TestKey";
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // HaiHM add DefaultChallengeScheme => Return 403 when conect resource can't not accept
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddCors();
            services.AddMvc();
            //services.AddTransient<CRM_MASTERContext>();
            services.AddDbContextPool<CRM_MASTERContext>(
            options => options
            .UseSqlServer(
                Configuration.GetSection("ConnectionStrings").GetSection("MASTERConnection").Value.ToString(),
                x => x.EnableRetryOnFailure())
            .ConfigureWarnings(x => x.Throw(RelationalEventId.QueryClientEvaluationWarning))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), 300);
            services.AddTransient<IAccountRepository, AccountDA>();
            services.AddTransient<IAuthorityRepository, AuthorityDA>();
            services.AddTransient<ILogUserRepository, LogUserLogDA>();
            services.AddTransient<IEmailService, EmailServiceEx>();
            services.AddTransient<IServicePackRepository, ServicePackDA>();
            //HaiHM Comment line under
            //AccountConstant.SQL_CONNECTION = Configuration.GetSection("ConnectionStrings").GetSection("MASTERConnection").Value.ToString();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Using get IP client
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            //Su dung httpcontext
            services.AddHttpContextAccessor();
            //Use Redis cache
            services.AddDistributedRedisCache(options =>
            { options.Configuration = AccountConstant.IpRedisCache; });
            //Use store procedure

            // Mail
            /*
            services.AddTransient<IEmailService, EmailServiceEx>();
            if (!EmailConstants.IsRunningEmailExecute)
                Task.Factory.StartNew(StartExecute);
            */
            //services.AddDbContext<MP_CRMContext>(options => options.UseSqlServer(Configuration.GetConnectionString("db_core_sp_call")));
            //Use Policy Role User - HaiHM
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AccountConstant.PolicyCanAdd, policy =>
                  policy.RequireRole(AccountConstant.CanAdd));

                options.AddPolicy(AccountConstant.PolicyEdit, policy =>
                  policy.RequireRole(AccountConstant.CanEdit, AccountConstant.CanEditAll));

                options.AddPolicy(AccountConstant.PolicyDelete, policy =>
                  policy.RequireRole(AccountConstant.CanDelete, AccountConstant.CanDeleteAll));

                options.AddPolicy(AccountConstant.PolicyShow, policy =>
                  policy.RequireRole(AccountConstant.CanShow, AccountConstant.CanShowAll));

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
            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
            //app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Account", action = "Login" });
            });
        }
    }
}
