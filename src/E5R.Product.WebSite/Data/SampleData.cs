// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Data.Entity;

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
                await context.Database.MigrateAsync();
                await CreateRootUser(serviceProvider);
            }
        }

        private static async Task CreateRootUser(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<ProductOptions>>().Value;
            var authContext = serviceProvider.GetRequiredService<AuthContext>(); 
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(ProductOptions.AUTH_ROOT_ROLE))
            {
                await roleManager.CreateAsync(new IdentityRole(ProductOptions.AUTH_ROOT_ROLE));
            }

            var rootUser = await userManager.FindByNameAsync(options.DefaultRootUser.UserName);

            if (rootUser == null)
            {
                rootUser = new Model.User
                {
                    UserName = options.DefaultRootUser.UserName
                };

                await userManager.CreateAsync(rootUser, options.DefaultRootUser.Password);
                await userManager.AddToRoleAsync(rootUser, ProductOptions.AUTH_ROOT_ROLE);
                await userManager.AddClaimAsync(rootUser, new Claim(ProductOptions.AUTH_ROOT_CLAIM, ProductOptions.AUTH_CLAIM_ALLOWED));
            }
        }
    }
}