using AutoMapper;
using Jobsity.Challenge.FinancialChat.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Jobsity.Challenge.FinancialChat.Interfaces.UseCases;

namespace Jobsity.Challenge.FinancialChat.UseCases.Services
{
    public class GetRoomUseCase : IGetRoomUseCase
    {
        private readonly IChatRoomRepository _chatRoomRepository;

        private readonly IMapper _mapper;

        public GetRoomUseCase(
                              IChatRoomRepository chatRoomRepository,
                              IMapper mapper)
        {
            _chatRoomRepository = chatRoomRepository ?? throw new ArgumentNullException(nameof(chatRoomRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ChatRoomDto>> GetAll()
        {
            var rooms = await _chatRoomRepository.GetAllRooms();
            return _mapper.Map<IEnumerable<ChatRoomDto>>(rooms);
        }

        public async Task<ChatRoomDto> GetByNameAndUser(string groupName, Guid userId)
        {
            var rooms = await _chatRoomRepository.GetRoomByNameAndUser(groupName, userId);
            return _mapper.Map<ChatRoomDto>(rooms);
        }
    }
}