﻿using AutoMapper;
using Jobsity.Challenge.FinancialChat.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Jobsity.Challenge.FinancialChat.Interfaces.UseCases;

namespace Jobsity.Challenge.FinancialChat.UseCases.UseCases
{
    public class SaveUserUseCase : ISaveUserUseCase
    {
        private readonly IUserConnectionRepository _userConnectionRepository;
        private readonly IMapper _mapper;

        public SaveUserUseCase(
                               IUserConnectionRepository userConnectionRepository,
                               IMapper mapper)
        {
            _userConnectionRepository = userConnectionRepository ?? throw new ArgumentNullException(nameof(userConnectionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDto> SaveUser(NewUserDto userDto, string connectionId)
        {
            var user = new User
            {
                ConnectionId = connectionId,
                DtConnection = userDto.DtConnection,
                Id = Guid.NewGuid(),
                Name = userDto.Name
            };

            await _userConnectionRepository.Save(user);
            return _mapper.Map<UserDto>(user);
        }
    }
}