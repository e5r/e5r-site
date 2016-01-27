// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace E5R.Product.WebSite.Data.Context
{
    using Model;
    
    public class AuthContext : IdentityDbContext<Model.User>
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProfile>(b => {
                b.HasKey(p => p.UserId);
                b.HasOne(p => p.User)
                 .WithOne(u => u.Profile);
            });
            
            builder.Entity<AskTheTeam>(b => {
                b.HasKey(att => att.UserId);
                b.HasOne(att => att.User)
                 .WithOne(u => u.AskTheTeam);
            });
        }
    }
}