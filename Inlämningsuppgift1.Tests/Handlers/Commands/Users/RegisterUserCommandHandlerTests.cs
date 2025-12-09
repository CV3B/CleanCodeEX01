using Inlämningsuppgift_1.Core.Commands.Users;
using Inlämningsuppgift_1.Core.Handlers.Commands.Users;
using Inlämningsuppgift_1.Core.Interfaces;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Commands.Users
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepository.Object);
            _handler = new RegisterUserCommandHandler(_mockUnitOfWork.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task Handle_ValidUser_RegistersSuccessfully()
        {
            var command = new RegisterUserCommand
            {
                Username = "testuser",
                Password = "Password123!",
                Email = "test@test.com"
            };

            _mockUserRepository.Setup(r => r.GetByUsername(command.Username)).Returns((User?)null);
            _mockPasswordHasher.Setup(h => h.HashPassword(command.Password)).Returns("hashedpassword");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _mockUserRepository.Verify(r => r.Add(It.Is<User>(u =>
                u.Username == command.Username &&
                u.Email == command.Email &&
                u.Password == "hashedpassword")), Times.Once);
        }

        [Fact]
        public async Task Handle_ExistingUsername_ReturnsFailure()
        {
            var command = new RegisterUserCommand
            {
                Username = "existinguser",
                Password = "Password123!",
                Email = "test@test.com"
            };

            _mockUserRepository.Setup(r => r.GetByUsername(command.Username)).Returns(new User());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ReturnsFailure()
        {
            var command = new RegisterUserCommand
            {
                Username = "newuser",
                Password = "short",
                Email = "test@test.com"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
        }
    }
}
