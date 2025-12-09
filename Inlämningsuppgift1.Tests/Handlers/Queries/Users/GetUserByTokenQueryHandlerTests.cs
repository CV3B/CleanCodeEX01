using Inlämningsuppgift_1.Core.Handlers.Queries.Users;
using Inlämningsuppgift_1.Core.Queries.Users;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Queries.Users
{
    public class GetUserByTokenQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ITokenRepository> _mockTokenRepository;
        private readonly GetUserByTokenQueryHandler _handler;

        public GetUserByTokenQueryHandlerTests()
        {

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenRepository = new Mock<ITokenRepository>();

            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.Setup(u => u.Tokens).Returns(_mockTokenRepository.Object);

            _handler = new GetUserByTokenQueryHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidToken_ReturnsUser()
        {
            var token = "valid-token";
            var userId = 1;
            var expectedUser = new User { Id = userId, Username = "testuser", Password = "hash" };

            _mockTokenRepository.Setup(t => t.GetUserIdByToken(token))
                .Returns(userId);
            _mockUserRepository.Setup(u => u.GetById(userId))
                .Returns(expectedUser);

            var query = new GetUserByTokenQuery { Token = token };
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(userId, result.Value.Id);
            Assert.Equal("testuser", result.Value.Username);
        }

        [Fact]
        public async Task Handle_InvalidToken_ReturnsFailure()
        {
            var token = "invalid-token";

            _mockTokenRepository.Setup(t => t.GetUserIdByToken(token))
                .Returns((int?)null);

            var query = new GetUserByTokenQuery { Token = token };
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}
