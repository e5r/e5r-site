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
            builder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.Entity<UserProfile>()
                .HasKey(p => p.UserId);
            
            base.OnModelCreating(builder);
        }
    }
}