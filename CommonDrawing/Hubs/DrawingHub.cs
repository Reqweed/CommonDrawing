using CommonDrawing.Models;
using CommonDrawing.Services;
using Microsoft.AspNetCore.SignalR;

namespace CommonDrawing.Hubs;

public class DrawingHub : Hub
{
    private static Dictionary<(string groupId, string listId), List<Shape>> _shapes = new();
    private static Dictionary<string, Dictionary<string, string>> _usersInGroups = new();
    private static Dictionary<string, Dictionary<string, bool>> _permissions = new();
    private readonly IGroupService _groupService;

    public DrawingHub(IGroupService groupService)
    {
        _groupService = groupService;
    }
    
    public async Task SendDrawing(Shape shape, string groupId, string listId)
    {
        var groupUsers = _permissions[groupId];

        if (groupUsers.ContainsKey(Context.UserIdentifier) && groupUsers[Context.UserIdentifier])
        {
            if (_shapes.TryGetValue((groupId, listId), out var shapes))
            {
                shapes.Add(shape);
            }
            else
            {
                _shapes[(groupId, listId)] = new List<Shape> { shape };
            }
            
            await Clients.OthersInGroup(groupId).SendAsync("Drawing", shape, listId);
        }
        else
        {
            await Clients.Caller.SendAsync("PermissionDenied", "Вам запрещено рисовать в этой группе.");
        }
    }

    public async Task GetPoints(string groupId)
    {
        var pairs = _shapes.Where(entry => entry.Key.groupId == groupId)
            .Select(entry => (entry.Key.listId, entry.Value))
            .OrderBy(p => int.Parse(p.listId))
            .ToList();
        if (pairs.Count > 0)
        {
            foreach (var pair in pairs)
            {
                await Clients.Caller.SendAsync("LoadDrawings", pair.Value, pair.listId);
            }
        }
    }
    
    public async Task AddList(string groupId, string listId)
    {
        _shapes[(groupId, listId)] = new List<Shape>();
        await Clients.OthersInGroup(groupId).SendAsync("AddList", listId);
    }

    public async Task RemoveList(string groupId, string listId)
    {
        _shapes.Remove((groupId, listId));
        await Clients.OthersInGroup(groupId).SendAsync("RemoveList", listId);
    }
    
    public async Task JoinGroup(string name, string userId, string? groupId = null, string? userName = null)
    {
        var group = groupId is null ? _groupService.CreateGroup(name, userId) : _groupService.GetGroup(Guid.Parse(groupId));

        await Groups.AddToGroupAsync(Context.ConnectionId, group.GroupId.ToString());
        
        if (!_usersInGroups.ContainsKey(group.GroupId.ToString()))
        {
            _usersInGroups[group.GroupId.ToString()] = new ();
            _permissions[group.GroupId.ToString()] = new ();
        }

        _usersInGroups[group.GroupId.ToString()][Context.UserIdentifier] = userName;

        bool canDraw = userId == group.OwnerId;
        _permissions[group.GroupId.ToString()][Context.UserIdentifier] = canDraw;

        await Clients.Group(group.GroupId.ToString()).SendAsync("UsersInGroup", _usersInGroups[group.GroupId.ToString()], group.OwnerId, _permissions[group.GroupId.ToString()]);
    }

    public async Task SetDrawingPermission(string userId, string groupId, bool canDraw)
    {
        var group = _groupService.GetGroup(Guid.Parse(groupId));

        var a = Context.UserIdentifier;

        _permissions[groupId][userId] = canDraw;
        await Clients.User(userId).SendAsync("PermissionChanged", canDraw, _usersInGroups[groupId], group.OwnerId, _permissions[groupId]);
        await Clients.Group(group.GroupId.ToString()).SendAsync("UsersInGroup", _usersInGroups[group.GroupId.ToString()], group.OwnerId, _permissions[group.GroupId.ToString()]);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        foreach (var group in _usersInGroups)
        {
            if (group.Value.ContainsKey(Context.UserIdentifier))
            {
                group.Value.Remove(Context.UserIdentifier);

                _permissions[group.Key].Remove(Context.UserIdentifier);

                var ownerId = _groupService.GetGroup(Guid.Parse(group.Key)).OwnerId;

                await Clients.Group(group.Key).SendAsync("UsersInGroup", group.Value, ownerId);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}