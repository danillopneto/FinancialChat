using AutoMapper;
using Jobsity.Challenge.FinancialChat.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Jobsity.Challenge.FinancialChat.Interfaces.UseCases;

namespace Jobsity.Challenge.FinancialChat.UseCases.UseCases
{
    public class SaveMessageIntoRoomUseCase : ISaveMessageIntoRoomUseCase
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IMapper _mapper;

        public SaveMessageIntoRoomUseCase(
                                          IChatRoomRepository chatRoomRepository,
                                          IMapper mapper)
        {
            _chatRoomRepository = chatRoomRepository ?? throw new ArgumentNullException(nameof(chatRoomRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task SaveAsync(ChatMessageDto chatMessageDto)
        {
            var chatMessage = _mapper.Map<ChatMessage>(chatMessageDto);
            await _chatRoomRepository.SaveNewMessageAsync(chatMessage);
        }
    }
}