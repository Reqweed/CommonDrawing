using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using CommonDrawing.Models;

namespace CommonDrawing.Services;

public class PrincipalProvider : IPrincipalProvider
{
    public ClaimsPrincipal GeneratePrincipal(User user)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.UserName),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        return principal;
    }
}
