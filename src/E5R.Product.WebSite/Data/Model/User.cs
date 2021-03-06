// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.AspNet.Identity.EntityFramework;

namespace E5R.Product.WebSite.Data.Model
{
    public class User : IdentityUser
    {
        public UserProfile Profile { get; set; }
        public AskTheTeam AskTheTeam { get; set; }
    }
}