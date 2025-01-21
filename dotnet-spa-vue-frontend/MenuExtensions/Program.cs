using MenuExtensions;
using MenuExtensions.Abstractions.Services;
using MenuExtensions.Services;
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
            context.Response.Redirect(Constants.Authorization.Endpoints.AccessDenied);
            return Task.CompletedTask;
        };
        setup.Cookie.SameSite = SameSiteMode.None;
        setup.ExpireTimeSpan = TimeSpan.FromMinutes(
            Constants.VismaConnect.SessionCookieLifetimeMinutes
        );
    })
    .AddOpenIdConnect(options =>
    {
        options.Authority = Constants.VismaConnect.OpenIdAuthority;
        options.CallbackPath = Constants.Authorization.Endpoints.Callback;
        // This value is set in the Details-menu when creating the application in Developer Portal
        options.ClientId = "<CLIENT_ID>";
        // This value can be generated in Developer Portal under the Credentials-menu
        options.ClientSecret = "<CLIENT_SECRET>";
        options.RemoteSignOutPath = Constants.Authorization.Endpoints.SignOut;
        options.ResponseType = OpenIdConnectResponseType.Code;
        // Access token is required when calling the different integrations that are found in the Developer Portal
        options.SaveTokens = true;
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        foreach (string scope in Constants.VismaConnect.Scopes)
        {
            options.Scope.Add(scope);
        }
        options.Events.OnRedirectToIdentityProvider = static context =>
        {
            string? tenantId;
            try
            {
                // The tenantId is passed as a query parameter from the frontend
                // The frontend receives the tenantId from the init-call inside Visma Net
                tenantId = context.Request.Query["tenantid"];
            }
            catch (Exception)
            {
                // TODO: Handle error properly when tenantId is not found
                return Task.CompletedTask;
            }
            // Small validation for tenantId
            if (!string.IsNullOrWhiteSpace(tenantId) && tenantId.Length > 10)
            {
                context.Response.Cookies.Append(
                    "tenantid",
                    tenantId ?? "",
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(
                            Constants.VismaConnect.SessionCookieLifetimeMinutes
                        ),
                    }
                );
                // When adding the tenant_hint parameter, the user will not be prompted to select the tenant
                // The tenant used in Visma Net will be used automatically instead
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

// HttpContextAccessor is required to get the access token from the HttpContext
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IVismaNetService, VismaNetService>();

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
