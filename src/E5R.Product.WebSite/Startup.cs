// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Entity;
using System.Linq;
using System;

namespace E5R.Product.WebSite
{
    using Abstractions.Business;
    using Abstractions.Services;
    using Core.Business;
    using Addins.Senders;
    using Data.Context;
    using Data.Model;
    using Data;
    
    public class Startup
    {
        const string CONFIG_FILE_GLOBAL = "webapp.json";
        const string CONFIG_FILE_ENVIRONMENT = "webapp.{ENV}.json";
        
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
        
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv, ILoggerFactory loggerFactory)
        {
            if (env.IsProduction())
            {
                loggerFactory.MinimumLevel = LogLevel.Error;
            }
            else if (env.IsStaging())
            {
                loggerFactory.MinimumLevel = LogLevel.Warning;
            }
            else
            {
                loggerFactory.MinimumLevel = LogLevel.Verbose;
                loggerFactory.AddConsole((n,l) => l >= loggerFactory.MinimumLevel);
            }
            
            Logger = loggerFactory.CreateLogger(this.GetType().Namespace.ToString());
            
            var selfFullName = this.GetType().FullName;

            if (selfFullName.LastIndexOf('.') > 0)
            {
                ProductNs = selfFullName
                    .Substring(0, selfFullName.LastIndexOf('.'))
                    .Replace('.', ':');
            }
            
            Logger.LogInformation($"RuntimeFramework: { appEnv.RuntimeFramework.FullName }");
            Logger.LogInformation($"BasePath: { appEnv.ApplicationBasePath }");
            Logger.LogInformation($"WebRootPath: { env.WebRootPath }");
            
            Logger.LogVerbose($"ProductNs = { ProductNs }");
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile(CONFIG_FILE_GLOBAL)
                .AddJsonFile(CONFIG_FILE_ENVIRONMENT.Replace("{ENV}", env.EnvironmentName), true);

            if (env.IsDevelopment())
            {
                Logger.LogVerbose("Development environment detected.");
                
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        private string ProductNs { get; set; } = "App";        
        private readonly string[] AcceptedDatabaseTypes = new string[] { "SQLite", "SQLServer" };

        public IConfiguration Configuration { get; set; }
        public ILogger Logger { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuring Database
            var dbType = Configuration[$"{ProductNs}:Database:Type"];
            var dbConnectionString = Configuration[$"{ProductNs}:Database:ConnectionString"];
            
            Logger.LogVerbose($"Database Type = {dbType}");
            Logger.LogVerbose($"Connection String = {dbConnectionString}");
            
            if (AcceptedDatabaseTypes.Count(c => string.Compare(c, dbType, true) == 0) != 1)
            {
                var error = new Exception("Invalid database type configuration.");
                Logger.LogCritical("Configure service error", error);
                throw error;
            }
            
            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                var error = new Exception("Invalid database connection string configuration.");
                Logger.LogCritical("Configure service error", error);
                throw error;
            }
            
            var ef = services.AddEntityFramework();

            if (string.Compare(dbType, "SQLite", true) == 0)
            {
                ef.AddSqlite().AddDbContext<AuthContext>(options =>
                {
                    options.UseSqlite(dbConnectionString);
                });
            }

            if (string.Compare(dbType, "SQLServer", true) == 0)
            {
                ef.AddSqlServer().AddDbContext<AuthContext>(options =>
                {
                    options.UseSqlServer(dbConnectionString);
                });
            }

            // Configuring Options
            services.AddOptions();
            
            services.Configure<ProductOptions>(options =>
            {
                options.DefaultRootUser.UserName = Configuration[$"{ProductNs}:Auth:DefaultRootUser:UserName"];
                options.DefaultRootUser.Password = Configuration[$"{ProductNs}:Auth:DefaultRootUser:Password"];
            });
            
            services.Configure<MailgunOptions>(options => {
                options.BaseAddress = Configuration[$"{ProductNs}:Addins:Mailgun:BaseAddress"];
                options.Domain = Configuration[$"{ProductNs}:Addins:Mailgun:Domain"];
                options.From = Configuration[$"{ProductNs}:Addins:Mailgun:From"];
                options.AppKey = Configuration[$"{ProductNs}:Addins:Mailgun:AppKey"];
            });

            // Configuring Identity
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // TODO: Explicit reading is necessary?
                options.Password.RequireDigit = bool.Parse(Configuration["Identity:Password:RequireDigit"] ?? options.Password.RequireDigit.ToString());
                options.Password.RequireLowercase = bool.Parse(Configuration["Identity:Password:RequireLowercase"] ?? options.Password.RequireLowercase.ToString());
                options.Password.RequireUppercase = bool.Parse(Configuration["Identity:Password:RequireUppercase"] ?? options.Password.RequireUppercase.ToString());
                options.Password.RequireNonLetterOrDigit = bool.Parse(Configuration["Identity:Password:RequireNonLetterOrDigit"] ?? options.Password.RequireNonLetterOrDigit.ToString());
                options.Password.RequiredLength = int.Parse(Configuration["Identity:Password:RequiredLength"] ?? options.Password.RequiredLength.ToString());
                
                IdentityCookieOptions.ApplicationCookieAuthenticationType = ProductOptions.AUTH_APPLICATION_COOKIE;
                options.Cookies.ApplicationCookieAuthenticationScheme = ProductOptions.AUTH_APPLICATION_COOKIE;
                options.Cookies.ApplicationCookie.AuthenticationScheme = ProductOptions.AUTH_APPLICATION_COOKIE;
                options.Cookies.ApplicationCookie.CookieName = ProductOptions.AUTH_COOKIE;
                options.Cookies.ApplicationCookie.LoginPath = new PathString("/account/signin");
                options.Cookies.ApplicationCookie.LogoutPath = new PathString("/account/signout");
                options.Cookies.ApplicationCookie.ReturnUrlParameter = "urlReturn";
            })
                .AddEntityFrameworkStores<AuthContext>()
                .AddDefaultTokenProviders();

            // Configuring ASP.NET and MVC
            services.AddSession();
            services.AddCaching();
            
            services.ConfigureRouting(routeOptions =>
            {
                routeOptions.AppendTrailingSlash = true;
                routeOptions.LowercaseUrls = true;
            });

            services.AddMvc();
            
            // Configuring Application Services
            services.AddTransient<IUserBusiness, UserBusiness>();
            services.AddTransient<IEmailSender, MailgunEmailSender>();
        }

        public void Configure(IApplicationBuilder application, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                application.UseDeveloperExceptionPage()
                    .UseStatusCodePages()
                    .UseRuntimeInfoPage();
            }
            
            application.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            application.UseSession()
                .UseStaticFiles()
                .UseIdentity()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=home}/{action=index}/{id?}"
                    );
                });

            SampleData.EnsureDatabaseAsync(application.ApplicationServices).Wait();
        }
    }
}