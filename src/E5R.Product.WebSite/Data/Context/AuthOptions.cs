// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

namespace E5R.Product.WebSite.Data.Context
{
    public class AuthOptions
    {
        public const string ROOT_ROLE = "RootAdministrator";
        public const string ROOT_CLAIM = "RootAdministratorClaim";
        public const string CLAIM_ALLOWED = "Allowed";
        public const string APPLICATION_COOKIE = "ApplicationCookie";
        public const string COOKIE = "Interop";
        
        public string DefaultRootUser { get; set; }
        public string DefaultRootPassword { get; set; }
    }
}