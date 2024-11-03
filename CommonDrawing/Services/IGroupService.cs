using CommonDrawing.Models;

namespace CommonDrawing.Services;

public interface IGroupService
{
    IEnumerable<Group> GetAllGroups();
    Group CreateGroup(string name, string ownerId);
    Group GetGroup(Guid id);
}
