using Jobsity.Challenge.FinancialChat.Domain.Constants;
using Jobsity.Challenge.FinancialChat.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Interfaces.UseCases;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Jobsity.Challenge.FinancialChat.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        #region " FIELDS "

        private readonly IAddToRoomUseCase _addToRoomUseCase;
        private readonly IGetRoomUseCase _getRoomUseCase;
        private readonly IGetUserUseCase _getUserUseCase;

        private readonly ILogger<ChatHub> _logger;

        private readonly ISaveUserUseCase _saveUserUseCase;

        #endregion " FIELDS "

        #region " CONSTRUCTOR "

        public ChatHub(
                       IAddToRoomUseCase addToRoomUseCase,
                       IGetRoomUseCase getRoomUseCase,
                       IGetUserUseCase getUserUseCase,
                       ISaveUserUseCase saveUserUseCase,
                       ILogger<ChatHub> logger)
        {
            _addToRoomUseCase = addToRoomUseCase ?? throw new ArgumentNullException(nameof(addToRoomUseCase));
            _getRoomUseCase = getRoomUseCase ?? throw new ArgumentNullException(nameof(getRoomUseCase));
            _getUserUseCase = getUserUseCase ?? throw new ArgumentNullException(nameof(getUserUseCase));
            _saveUserUseCase = saveUserUseCase ?? throw new ArgumentNullException(nameof(saveUserUseCase));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion " CONSTRUCTOR "

        #region " PUBLIC METHODS "

        public async Task AddToChatRoom(string groupName)
        {
            try
            {
                var user = await _getUserUseCase.GetUserByConnectionId(Context.ConnectionId);
                var room = await _addToRoomUseCase.AddAsync(groupName, user);
                if (room is not null)
                {
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
                var userDto = JsonConvert.DeserializeObject<NewUserDto>(Context.GetHttpContext().Request.Query["user"]);
                var user = await _saveUserUseCase.SaveUser(userDto, Context.ConnectionId);

                await Clients.Caller.SendAsync("userData", user);

                await InsertCurrentUserIntoAvailableRooms();

                await Clients.All.SendAsync(ConstantsHubs.DefaultChat, await _getRoomUseCase.GetAll(), user);
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to connect the user.");
            }
        }

        /*public async Task RemoveFromChatRoom(string groupName)
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
        }*/

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

        #endregion " PUBLIC METHODS "

        #region " PRIVATE METHODS "

        private async Task InsertCurrentUserIntoAvailableRooms()
        {
            var rooms = await _getRoomUseCase.GetAll();
            foreach (var room in rooms)
            {
                await AddToChatRoom(room.Name);
            }
        }

        private async Task NotifyChatRoomOfLeftUser(string groupName, UserDto user, ChatRoomDto room)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id.ToString());
            await Clients.All.SendAsync(ConstantsHubs.DefaultChat, await _getRoomUseCase.GetAll(), user);
            await SendMessageToTheGroup(new ChatMessage(room.Id, $"{user.Name} has left the group {groupName}."));
        }

        private async Task NotifyChatRoomOfNewUser(string groupName, UserDto user, ChatRoomDto room)
        {
            await Clients.All.SendAsync(ConstantsHubs.DefaultChat, await _getRoomUseCase.GetAll(), user);
            await SendMessageToTheGroup(new ChatMessage(room.Id, $"{user.Name} has joined the group {groupName}."));
        }

        private async Task SendMessageToTheGroup(ChatMessage chat)
        {
            await Clients.Group(chat.Destination.ToString()).SendAsync("Receive", chat);
        }

        #endregion " PRIVATE METHODS "
    }
}