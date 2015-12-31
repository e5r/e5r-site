using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace E5R.Product.WebSite
{
    using Data;
    using Data.Context;
    using Data.Model;

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
                    options.UseSqlite(Configuration["Data:ConnectionStrings:Auth"]);
                });

            services.Configure<AuthOptions>(options =>
            {
                options.DefaultRootUser = Configuration["Auth:DefaultRootUser"];
                options.DefaultRootPassword = Configuration["Auth:DefaultRootPassword"];
            });

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Cookies.ApplicationCookieAuthenticationScheme = AuthOptions.APPLICATION_COOKIE;
                options.Cookies.ApplicationCookie.AuthenticationScheme = IdentityCookieOptions.ApplicationCookieAuthenticationType = AuthOptions.APPLICATION_COOKIE;
                options.Cookies.ApplicationCookie.CookieName = AuthOptions.COOKIE;
            })
            .AddEntityFrameworkStores<AuthContext>()
            .AddDefaultTokenProviders();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage()
                .UseStaticFiles()
                .UseIdentity()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}",
                        defaults: new { controller = "home", action = "index" }
                    );
                });
                
               SampleData.InitializeDatabaseAsync(app.ApplicationServices).Wait();
        }
    }
}