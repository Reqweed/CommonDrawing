namespace CommonDrawing.Models;

public class User(string userName)
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string? UserName { get; set; } = userName;
}
