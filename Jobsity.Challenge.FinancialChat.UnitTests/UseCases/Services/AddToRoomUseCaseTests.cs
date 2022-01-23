using AutoMapper;
using FluentAssertions;
using Jobsity.Challenge.FinancialChat.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Jobsity.Challenge.FinancialChat.UseCases.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jobsity.Challenge.FinancialChat.UnitTests.UseCases.Services
{
    public class AddToRoomUseCaseTests
    {
        private readonly AddToRoomUseCase _addToRoomUseCaseTests;

        private readonly Mock<IChatRoomRepository> _chatRoomRepository;

        private readonly Mock<IMapper> _mapper;

        private readonly Mock<IUserConnectionRepository> _userConnectionRepository;

        public AddToRoomUseCaseTests()
        {
            _chatRoomRepository = new();
            _userConnectionRepository = new();
            _mapper = new();
            _addToRoomUseCaseTests = new AddToRoomUseCase(
                                                          _chatRoomRepository.Object,
                                                          _userConnectionRepository.Object,
                                                          _mapper.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNull_WhenAlreadyHasUser()
        {
            _userConnectionRepository
                .Setup(u => u.GetUser(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<User>);
            _chatRoomRepository
                .Setup(c => c.HasUser(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(true);

            var result = await _addToRoomUseCaseTests.AddAsync(It.IsAny<string>(), It.IsAny<Guid>());

            result.Should().BeNull();
            _userConnectionRepository.Verify(
                                             u => u.GetUser(It.IsAny<Guid>()),
                                             Times.Once());
            _chatRoomRepository.Verify(
                                       c => c.HasUser(It.IsAny<string>(), It.IsAny<User>()),
                                       Times.Once());
        }

        [Fact]
        public async Task AddAsync_ShouldReturnRoom_WhenIsNewUser()
        {
            _userConnectionRepository
                .Setup(u => u.GetUser(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<User>);
            _chatRoomRepository
                .Setup(c => c.HasUser(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(false);
            _chatRoomRepository
                .Setup(c => c.AddUser(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(new ChatRoom());
            _mapper
                .Setup(m => m.Map<ChatRoomDto>(It.IsAny<ChatRoom>()))
                .Returns(new ChatRoomDto());

            var result = await _addToRoomUseCaseTests.AddAsync(It.IsAny<string>(), It.IsAny<Guid>());

            result.Should().NotBeNull();
            _userConnectionRepository.Verify(
                                             u => u.GetUser(It.IsAny<Guid>()),
                                             Times.Once());
            _chatRoomRepository.Verify(
                                       c => c.HasUser(It.IsAny<string>(), It.IsAny<User>()),
                                       Times.Once());
            _chatRoomRepository.Verify(
                                       c => c.AddUser(It.IsAny<string>(), It.IsAny<User>()),
                                       Times.Once());
            _mapper.Verify(
                           m => m.Map<ChatRoomDto>(It.IsAny<ChatRoom>()),
                           Times.Once());
        }
    }
}