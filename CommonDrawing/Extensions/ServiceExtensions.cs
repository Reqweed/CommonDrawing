using Microsoft.AspNetCore.Authentication.Cookies;
using CommonDrawing.Services;

namespace CommonDrawing.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureAuth(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.Cookie.Name = "OnlinePresentationCookie";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            });
    }

    public static void ConfigureSignalR(this IServiceCollection services)
    {
        services.AddSignalR(options =>
        {
            options.MaximumReceiveMessageSize = 1024 * 1024 * 1024;
        });
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPrincipalProvider, PrincipalProvider>();
        services.AddSingleton<IGroupService, GroupService>();
    }
}