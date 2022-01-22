using Jobsity.Challenge.FinancialChat.Domain.Constants;
using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Infra.Repositories;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Jobsity.Challenge.FinancialChat.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConnectionsRepository _connections = new ConnectionsRepository();

        private static readonly ChatRoomRepository _rooms = new ChatRoomRepository();

        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddToGroup(string groupName)
        {
            try
            {
                var user = _connections.GetUserByConnectionId(Context.ConnectionId);
                if (!_rooms.HasUser(groupName, user).GetValueOrDefault())
                {
                    var room = _rooms.AddUser(groupName, user);

                    await Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString());
                    await NotifyChatRoomOfNewUser(groupName, user, room);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to add the user to the group {GroupName}.", groupName);
            }
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = JsonConvert.DeserializeObject<User>(Context.GetHttpContext().Request.Query["user"]);
                _connections.Save(user, Context.ConnectionId);

                await Clients.Caller.SendAsync("userData", user);

                await InsertCurrentUserIntoAvailableRooms();

                await Clients.All.SendAsync(ConstantsHubs.DefaultChat, _rooms.GetAllRooms(), user);
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to connect the user.");
            }
        }

        public async Task RemoveFromGroup(string groupName)
        {
            try
            {
                var user = _connections.GetUserByConnectionId(Context.ConnectionId);
                _rooms.RemoveUser(groupName, user);
                var room = _rooms.GetRoomByName(groupName);

                await NotifyChatRoomOfLeftUser(groupName, user, room);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to remove the user to the group {GroupName}.", groupName);
            }
        }

        public async Task SendMessage(ChatMessage chat)
        {
            try
            {
                await SendMessageToTheGroup(chat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to send a message to the group {GroupName}.", chat?.Destination);
            }
        }

        #region " PRIVATE METHODS "

        private async Task InsertCurrentUserIntoAvailableRooms()
        {
            var rooms = _rooms.GetAllRooms();
            foreach (var room in rooms)
            {
                await AddToGroup(room.Name);
            }
        }

        private async Task NotifyChatRoomOfLeftUser(string groupName, User user, ChatRoom room)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id.ToString());
            await Clients.All.SendAsync(ConstantsHubs.DefaultChat, _rooms.GetAllRooms(), user);
            await SendMessageToTheGroup(new ChatMessage(room.Id, $"{user.Name} has left the group {groupName}."));
        }

        private async Task NotifyChatRoomOfNewUser(string groupName, User user, ChatRoom room)
        {
            await Clients.All.SendAsync(ConstantsHubs.DefaultChat, _rooms.GetAllRooms(), user);
            await SendMessageToTheGroup(new ChatMessage(room.Id, $"{user.Name} has joined the group {groupName}."));
        }

        private async Task SendMessageToTheGroup(ChatMessage chat)
        {
            await Clients.Group(chat.Destination.ToString()).SendAsync("Receive", chat);
        }

        #endregion " PRIVATE METHODS "
    }
}