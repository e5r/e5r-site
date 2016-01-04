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