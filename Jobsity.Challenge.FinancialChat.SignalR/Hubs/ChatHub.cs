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

        public override Task OnConnectedAsync()
        {
            var user = JsonConvert.DeserializeObject<User>(Context.GetHttpContext().Request.Query["user"]);
            _connections.Add(Context.ConnectionId, user);

            Clients.All.SendAsync(ConstantsHubs.DefaultChat, _rooms.GetAllRooms(), user);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Clients.All.SendAsync(ConstantsHubs.DefaultChat, _connections.GetAllUser());
            return base.OnDisconnectedAsync(exception);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var user = _connections.GetUser(Context.ConnectionId);
            _rooms.AddUser(groupName, user);

            await Clients.All.SendAsync(ConstantsHubs.DefaultChat, _rooms.GetAllRooms(), user);
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public async Task SendMessage(ChatMessage chat)
        {
            await Clients.Client(_connections.GetUserId(chat.Destination)).SendAsync("Receive", chat.Sender, chat.Message);
        }
    }
}