using CommonDrawing.Models;

namespace CommonDrawing.Services;

public class GroupService : IGroupService
{
    private readonly Dictionary<Guid, Group> _groups;

    public GroupService()
    {
        _groups = new Dictionary<Guid, Group>();
    }

    public IEnumerable<Group> GetAllGroups()
    {
        return _groups.Values;
    }

    public Group CreateGroup(string name, string ownerId)
    {
        var group = new Group(name, ownerId);
        _groups[group.GroupId] = group;
        return group;
    }

    public Group GetGroup(Guid id)
    {
        if (!_groups.TryGetValue(id, out Group? group))
        {
            return null;
            // throw new GroupNotFoundException(id);
        }

        return group;
    }
}
