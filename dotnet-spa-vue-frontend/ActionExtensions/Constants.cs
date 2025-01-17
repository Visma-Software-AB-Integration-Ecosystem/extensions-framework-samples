namespace ActionExtensions;

public static class Constants
{
    public static class Authorization
    {
        public static class Endpoints
        {
            // The /vismaconnect-endpoints are mapped to the VismaConnectController inside of this project
            /// <summary>
            /// Redirects the user to the access denied page.
            /// </summary>
            public const string AccessDenied = "/vismaconnect/loginfailed";

            /// <summary>
            /// Redirects the user to the callback page.
            /// </summary>
            public const string Callback = "/vismaconnect/callback/";

            /// <summary>
            /// Redirects the user to the sign out page.
            /// </summary>
            public const string SignOut = "/vismaconnect/signout/";

            /// <summary>
            /// The token endpoint for Visma Connect.
            /// </summary>
            public const string Token = "/connect/token";
        }
    }

    public static class VismaConnect
    {
        /// <summary>
        /// The lifetime of the session cookie in minutes.
        /// This value should be less than or equal to the Visma Connect access token lifespan.
        /// The access token lifespan is configured in the Details-menu in Developer Portal.
        /// </summary>
        public const int SessionCookieLifetimeMinutes = 60;

        // TODO: Handle different environments
        // This URL is specific for staging environment. For production, it should be https://connect.visma.com
        public const string OpenIdAuthority = "https://connect.identity.stagaws.visma.com";

        // The vismanet_erp_interactive_api-scopes specified are required for the usage inside VismaConnectService
        // They can be added and removed as needed based on your specific requirements
        // The scopes are configured in Developer Portal
        /// <summary>
        /// The scopes used for the Visma Connect OpenIDConnect API and integrations specified in the Developer Portal.
        /// </summary>
        public static readonly IEnumerable<string> Scopes =
        [
            "openid",
            "profile",
            "email",
            "vismanet_erp_interactive_api:read",
            "vismanet_erp_interactive_api:create",
        ];
    }
}
