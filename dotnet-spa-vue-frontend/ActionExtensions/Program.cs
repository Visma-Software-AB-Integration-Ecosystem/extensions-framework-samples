using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(setup =>
    {
        setup.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.Redirect("/vismaconnect/loginfailed");
            return Task.CompletedTask;
        };
        setup.Cookie.SameSite = SameSiteMode.Lax;
        setup.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        setup.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    })
    .AddOpenIdConnect(options =>
    {
        // TODO: Handle different environments
        // For Production, this value is supposed to be: https://connect.visma.com
        // The value below is for the staging environment
        string authority = "https://connect.identity.stagaws.visma.com";

        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.Authority = authority;
        options.CallbackPath = "/vismaconnect/callback/";
        options.RemoteSignOutPath = "/vismaconnect/signout/";
        // options.ClientId = "<Developer Portal Client ID>";
        // options.ClientSecret = "<Developer Portal Secret>";
        options.ClientId = "<CLIENT_ID>";
        options.ClientSecret = "<CLIENT_SECRET>";
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.ResponseMode = OpenIdConnectResponseMode.FormPost;
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        // Scope openid and profile are added by default. Add more scopes if needed.
        // options.Scope.Add("tenants");
        // options.ClaimActions.MapJsonKey("tenants", "tenants");
        options.Events.OnRedirectToIdentityProvider = context =>
        {
            string? tenantId;
            try
            {
                tenantId = context.Request.Query["tenantid"];
            }
            catch (System.Exception)
            {
                return Task.CompletedTask;
            }
            // Small validation for tenantId
            if (!string.IsNullOrWhiteSpace(tenantId) && tenantId.Length > 10)
            {
                context.Response.Cookies.Append(
                    "tenantid",
                    tenantId ?? "",
                    new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(60)
                    }
                );
                context.ProtocolMessage.Parameters.Add("tenant_hint", tenantId);
            }
            return Task.CompletedTask;
        };
    });

// Add the files that should be served when visiting the root path of the application
builder.Services.AddSpaStaticFiles(config =>
{
    config.RootPath = "frontend/dist";
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Serve the files from the frontend/dist folder
app.UseSpaStaticFiles();
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "frontend";
});

app.Run();
