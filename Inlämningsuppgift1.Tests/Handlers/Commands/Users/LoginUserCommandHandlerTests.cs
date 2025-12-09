using Inlämningsuppgift_1.Core.Commands.Users;
using Inlämningsuppgift_1.Core.Handlers.Commands.Users;
using Inlämningsuppgift_1.Core.Interfaces;
using Inlämningsuppgift_1.Data.Constants;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Commands.Users
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ITokenRepository> _mockTokenRepository;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenRepository = new Mock<ITokenRepository>();

            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.Setup(u => u.Tokens).Returns(_mockTokenRepository.Object);

            _handler = new LoginUserCommandHandler(_mockUnitOfWork.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsToken()
        {
            var command = new LoginUserCommand
            {
                Username = "testuser",
                Password = "password123"
            };
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Password = "hashedpassword"
            };

            _mockUserRepository.Setup(r => r.GetByUsername("testuser")).Returns(user);
            _mockPasswordHasher.Setup(h => h.VerifyPassword("hashedpassword", "password123")).Returns(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.StartsWith(TokenSettings.TokenPrefix, result.Value);
        }

        [Fact]
        public async Task Handle_InvalidUsername_ReturnsFailure()
        {
            var command = new LoginUserCommand { Username = "nonexistent", Password = "password123" };

            _mockUserRepository.Setup(r => r.GetByUsername("nonexistent")).Returns((User?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ReturnsFailure()
        {
            var command = new LoginUserCommand
            {
                Username = "testuser",
                Password = "wrongpassword"
            };
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Password = "hashedpassword"
            };

            _mockUserRepository.Setup(r => r.GetByUsername("testuser")).Returns(user);
            _mockPasswordHasher.Setup(h => h.VerifyPassword("hashedpassword", "wrongpassword")).Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
        }
    }

}
