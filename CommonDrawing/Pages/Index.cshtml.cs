using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using CommonDrawing.Hubs;
using CommonDrawing.Models;
using CommonDrawing.Services;

namespace CommonDrawing.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IHubContext<DrawingHub> _hubContext;
    private readonly IGroupService _groupService;
    public IEnumerable<Group> Groups { get; }
    [BindProperty]
    public string GroupName { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }

    public IndexModel(IHubContext<DrawingHub> hubContext, IGroupService groupService)
    {
        _hubContext = hubContext;
        _groupService = groupService;
        Groups = _groupService.GetAllGroups();
    }

    public void OnGet()
    {
        UserName = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<IActionResult> OnGetLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToPage("Login");
    }

    public IActionResult OnPost()
    {
        return RedirectToPage(nameof(Index));
    }
}
