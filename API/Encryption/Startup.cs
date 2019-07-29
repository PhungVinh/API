using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encryption.Constant;
using Encryption.ContextFactory;
using Encryption.DataAccess;
using Encryption.Models;
using Encryption.Options;
using Encryption.Repositories;
using Encryption.Scheduler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.Impl;

namespace Encryption
{
    public class Startup
    {
        private readonly IScheduler scheduler;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            scheduler = EncrpytionScheduler();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<Jwt>(Configuration);
            // Add Dbcontext and DI
            services.AddTransient<CRM_MPContext>();
            services.AddTransient<IEncryptionRepository, EncryptionDA>();
            // Add cache Redis
            services.AddDistributedRedisCache(opts =>
            {
                opts.Configuration = EncryptionConstant.IP;
            });
            // Add httpContext
            services.AddHttpContextAccessor();
            // Add JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration[EncryptionConstant.Key])),
                        ValidateIssuer = true,
                        ValidIssuer = Configuration[EncryptionConstant.Issuer],
                        ValidateAudience = true,
                        ValidAudience = Configuration[EncryptionConstant.Issuer],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = true,
                    };
                });
            // Add schedule service
            //services.AddHostedService<ScheduleService>();
            //services.AddScoped<IHostedService, ScheduleService>();
            services.AddSingleton(provider => scheduler);
            services.AddTransient<EncryptionJob>();
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
            scheduler.JobFactory = new EncryptionJobFactory(app.ApplicationServices);
            // Multiple database
            Dictionary<string, string> dConnectionStrings = new Dictionary<string, string>();
            var lstConnectionStrings = Configuration.GetSection(EncryptionConstant.ConnectionStrings).AsEnumerable().ToList();
            if (lstConnectionStrings != null)
            {
                for (int i = 1; i < lstConnectionStrings.Count(); i++)
                {
                    dConnectionStrings.Add(lstConnectionStrings[i].Key.Replace(EncryptionConstant.ConnectionStrings, string.Empty), lstConnectionStrings[i].Value);

                }
            }
            DbContextFactory dbContextFactory = DbContextFactory.getInstance(dConnectionStrings);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
        private IScheduler EncrpytionScheduler()
        {
            var properties = new NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = "Encrpytion",
                ["quartz.threadPool.threadCount"] = "1"
            };
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.Start();
            return scheduler;
        }
    }
}
