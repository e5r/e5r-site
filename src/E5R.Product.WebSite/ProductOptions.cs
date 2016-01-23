// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

namespace E5R.Product.WebSite
{
    public class DefaultRootUserOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    
    public class DatabaseOptions {
        public string Type { get; set; }
        public string ConnectionString { get; set; }
    }
    
    public class ProductOptions
    {
        public const string AUTH_ROOT_ROLE = "RootAdministrator";
        public const string AUTH_ROOT_CLAIM = "RootAdministratorClaim";
        public const string AUTH_CLAIM_ALLOWED = "Allowed";
        public const string AUTH_APPLICATION_COOKIE = "ApplicationCookie";
        public const string AUTH_COOKIE = "Interop";
        public const string AUTH_USER_TEMP_PREFIX = "tempuser";
                
        public DefaultRootUserOptions DefaultRootUser { get; set; } = new DefaultRootUserOptions();
        public DatabaseOptions Database { get; set; } = new DatabaseOptions();
    }
}