using System.Security.Claims;
using CommonDrawing.Models;

namespace CommonDrawing.Services;

public interface IPrincipalProvider
{
    ClaimsPrincipal GeneratePrincipal(User user);
}