// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace E5R.Product.WebSite
{
    using Data;
    using Data.Context;
    using Data.Model;
    using Microsoft.AspNet.Http;

    public class Startup
    {
        const string CONFIG_FILE_GLOBAL = "webapp.json";
        const string CONFIG_FILE_ENVIRONMENT = "webapp.{ENV}.json";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(CONFIG_FILE_GLOBAL)
                .AddJsonFile(CONFIG_FILE_ENVIRONMENT.Replace("{ENV}", env.EnvironmentName), true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFramework()
                .AddSqlite()
                .AddDbContext<AuthContext>(options =>
                {
                    options.UseSqlite(Configuration["Auth:ConnectionString"]);
                });

            services.Configure<AuthOptions>(options =>
            {
                options.DefaultRootUser = Configuration["Auth:DefaultRootUser"];
                options.DefaultRootPassword = Configuration["Auth:DefaultRootPassword"];
            });

            services.AddIdentity<User, IdentityRole>(options =>
            {
                IdentityCookieOptions.ApplicationCookieAuthenticationType = AuthOptions.APPLICATION_COOKIE;
                options.Cookies.ApplicationCookieAuthenticationScheme = AuthOptions.APPLICATION_COOKIE;
                options.Cookies.ApplicationCookie.AuthenticationScheme = AuthOptions.APPLICATION_COOKIE;
                options.Cookies.ApplicationCookie.CookieName = AuthOptions.COOKIE;
                options.Cookies.ApplicationCookie.LoginPath = new PathString("/account/signin");
                options.Cookies.ApplicationCookie.LogoutPath = new PathString("/account/signout");
                //options.Cookies.ApplicationCookie.ReturnUrlParameter = "";
            })
                .AddEntityFrameworkStores<AuthContext>()
                .AddDefaultTokenProviders();

            services.ConfigureRouting(routeOptions =>
            {
                //routeOptions.AppendTrailingSlash = true;
                routeOptions.LowercaseUrls = true;
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder application, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(LogLevel.Verbose);
                
                application.UseDeveloperExceptionPage()
                    .UseStatusCodePages()
                    .UseRuntimeInfoPage();
            }

            application.UseStaticFiles()
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