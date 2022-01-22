using AutoMapper;
using Jobsity.Challenge.FinancialChat.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Jobsity.Challenge.FinancialChat.Interfaces.UseCases;

namespace Jobsity.Challenge.FinancialChat.UseCases.UseCases
{
    public class AddToRoomUseCase : IAddToRoomUseCase
    {
        private readonly IChatRoomRepository _chatRoomRepository;

        private readonly IMapper _mapper;

        public AddToRoomUseCase(
                                IChatRoomRepository chatRoomRepository,
                                IMapper mapper)
        {
            _chatRoomRepository = chatRoomRepository ?? throw new ArgumentNullException(nameof(chatRoomRepository));
            _mapper = mapper;
        }

        public async Task<ChatRoomDto?> AddAsync(string roomName, UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            if (!await _chatRoomRepository.HasUser(roomName, user))
            {
                var room = await _chatRoomRepository.AddUser(roomName, user);
                return _mapper.Map<ChatRoomDto>(room);
            }

            return default;
        }
    }
}