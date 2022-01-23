using AutoMapper;
using Jobsity.Challenge.FinancialChat.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Jobsity.Challenge.FinancialChat.Interfaces.UseCases;

namespace Jobsity.Challenge.FinancialChat.UseCases.Services
{
    public class GetUserUseCase : IGetUserUseCase
    {
        private readonly IMapper _mapper;

        private readonly IUserConnectionRepository _userConnectionRepository;

        public GetUserUseCase(
                              IUserConnectionRepository userConnectionRepository,
                              IMapper mapper)
        {
            _userConnectionRepository = userConnectionRepository ?? throw new ArgumentNullException(nameof(userConnectionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDto> GetUserByConnectionId(string connectionId)
        {
            var user = await _userConnectionRepository.GetUserByConnectionId(connectionId);
            return _mapper.Map<UserDto>(user);
        }
    }
}