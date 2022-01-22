using AutoMapper;
using Jobsity.Challenge.FinancialChat.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Domain.Entities;

namespace Jobsity.Challenge.FinancialChat.UseCases.Automappers
{
    public class ConnectionsProfile : Profile
    {
        public ConnectionsProfile()
        {
            CreateMap<NewUserDto, User>()
                    .ForMember(
                               d => d.ConnectionId,
                               opt => opt.Ignore());
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<ChatRoom, ChatRoomDto>().ReverseMap();
        }
    }
}