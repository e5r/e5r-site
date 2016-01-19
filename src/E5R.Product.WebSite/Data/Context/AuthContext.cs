// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace E5R.Product.WebSite.Data.Context
{
    using Model;
    
    public class AuthContext : IdentityDbContext<Model.User>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().Property(m => m.FirstName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Entity<User>().Property(m => m.LastName)
                .IsRequired()
                .HasMaxLength(60);
                
            base.OnModelCreating(builder);
        }
    }
}