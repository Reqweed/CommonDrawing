using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommonDrawing.Models;
using CommonDrawing.Services;

namespace CommonDrawing.Pages;

[Authorize]
public class GroupModel : PageModel
{
    private readonly IAntiforgery _antiforgery;
    private readonly IGroupService _groupService;
    public Group Group { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }

    public GroupModel(IAntiforgery antiforgery, IGroupService groupService)
    {
        _antiforgery = antiforgery;
        _groupService = groupService;
    }

    public async void OnGet([FromQuery] string id)
    {
        Group = _groupService.GetGroup(Guid.Parse(id));
        UserName = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
        ViewData["XSRF-TOKEN"] = tokens.RequestToken;
    }
}
