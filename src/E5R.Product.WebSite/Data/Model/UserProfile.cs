// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

namespace E5R.Product.WebSite.Data.Model
{
    public class UserProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }
    }
}