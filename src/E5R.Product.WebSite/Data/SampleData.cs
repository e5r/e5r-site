using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;

namespace E5R.Product.WebSite.Data
{
    using Context;
    using Model;

    public class SampleData
    {
        public static async Task EnsureDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<AuthContext>())
            {
                if (await context.Database.EnsureCreatedAsync())
                {
                    await CreateRootUser(serviceProvider);
                }
            }
        }

        private static async Task CreateRootUser(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<AuthOptions>>().Value;

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(AuthOptions.ROOT_ROLE))
            {
                await roleManager.CreateAsync(new IdentityRole(AuthOptions.ROOT_ROLE));
            }

            var rootUser = await userManager.FindByNameAsync(options.DefaultRootUser);

            if (rootUser == null)
            {
                rootUser = new Model.User { UserName = options.DefaultRootUser };

                await userManager.CreateAsync(rootUser, options.DefaultRootPassword);
                await userManager.AddToRoleAsync(rootUser, AuthOptions.ROOT_ROLE);
                await userManager.AddClaimAsync(rootUser, new Claim(AuthOptions.ROOT_CLAIM, AuthOptions.CLAIM_ALLOWED));
            }
        }
    }
}