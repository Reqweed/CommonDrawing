namespace CommonDrawing.Models;

public class Group(string name, string ownerId)
{
    public Guid GroupId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string? OwnerId { get; set; } = ownerId;
    public List<User> Users { get; set; } = new();
}
