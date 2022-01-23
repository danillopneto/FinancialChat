using AutoMapper;
using Jobsity.Challenge.FinancialChat.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Jobsity.Challenge.FinancialChat.Interfaces.UseCases;

namespace Jobsity.Challenge.FinancialChat.UseCases.Services
{
    public class AddToRoomUseCase : IAddToRoomUseCase
    {
        private readonly IChatRoomRepository _chatRoomRepository;

        private readonly IMapper _mapper;

        private readonly IUserConnectionRepository _userConnectionRepository;

        public AddToRoomUseCase(
                                IChatRoomRepository chatRoomRepository,
                                IUserConnectionRepository userConnectionRepository,
                                IMapper mapper)
        {
            _chatRoomRepository = chatRoomRepository ?? throw new ArgumentNullException(nameof(chatRoomRepository));
            _userConnectionRepository = userConnectionRepository ?? throw new ArgumentNullException(nameof(userConnectionRepository));
            _mapper = mapper;
        }

        public async Task<ChatRoomDto?> AddAsync(string roomName, Guid userId)
        {
            var user = await _userConnectionRepository.GetUser(userId);
            if (!await _chatRoomRepository.HasUser(roomName, user))
            {
                var room = await _chatRoomRepository.AddUser(roomName, user);
                return _mapper.Map<ChatRoomDto>(room);
            }

            return default;
        }
    }
}