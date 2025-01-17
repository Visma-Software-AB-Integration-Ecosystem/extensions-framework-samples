using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionExtensions.Controllers;

[ApiController]
[Route("[controller]")]
public class VismaConnectController : ControllerBase
{
    /// <summary>
    /// This method is used to open a popup window that is used to verify that the user is authenticated to Visma Connect.
    /// </summary>
    /// <returns>IActionResult</returns>
    [HttpGet]
    [Route("popuplogin")]
    [Authorize]
    public async Task<IActionResult> PopupLogin([FromQuery] string? tenantId)
    {
        // Small validation for tenantId
        if (tenantId is not null && tenantId != HttpContext.Request.Cookies["tenantid"])
        {
            await HttpContext.ChallengeAsync();
        }

        return new ContentResult
        {
            // This is the HTML that is returned when the user is authenticated.
            // It shows up in the popup window that opens quickly when signing in.
            Content =
                @"
            <!doctype html>
            <html lang='en'>
            <head>
            <title>Visma Connect Authentication process</title>
            <script>
            function confirmLogin() {{
                window.opener.popupConfirmation();
                window.close();
            }}
            confirmLogin();
            </script>
            </head>
            <body>
                <p>Authenticating...</p>
            </body>
            </html>
            ",
            ContentType = "text/html"
        };
    }

    /// <summary>
    /// This method handles the callback required when confirming Visma Connect authentication.
    /// </summary>
    /// <returns>IActionResult</returns>
    [HttpPost]
    [Route("callback")]
    [AllowAnonymous]
    public IActionResult Callback()
    {
        return Ok();
    }

    /// <summary>
    /// This method is used to handle the login failed scenario.
    /// </summary>
    /// <returns>IActionResult</returns>
    [HttpGet]
    [Route("loginfailed")]
    [AllowAnonymous]
    public IActionResult LoginFailed()
    {
        return new ContentResult
        {
            // TODO: Add access denied HTML page
        };
    }

    /// <summary>
    /// This method is used to sign out the user.
    /// </summary>
    /// <returns>IActionResult</returns>
    [HttpGet]
    [Route("signout")]
    [AllowAnonymous]
    public async Task<IActionResult> SignOutGet()
    {
        // TODO: Handle different environments
        // For Production, this value is supposed to be: https://connect.visma.com
        // The value below is for the staging environmentC
        string vismaConnectOrigin = "https://connect.identity.stagaws.visma.com";

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Headers.Append(
            "Content-security-policy",
            $"frame-ancestors 'self' {vismaConnectOrigin}"
        );
        Response.Headers.Append("X-Frame-Options", $"allow-from {vismaConnectOrigin}");
        return NoContent();
    }
}
