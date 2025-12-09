using Inlämningsuppgift_1.Core.Commands.Carts;
using Inlämningsuppgift_1.Core.Handlers.Commands.Carts;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Commands.Carts
{
    public class ClearCartCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly ClearCartCommandHandler _handler;

        public ClearCartCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCartRepository = new Mock<ICartRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUnitOfWork.Setup(u => u.Carts).Returns(_mockCartRepository.Object);
            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepository.Object);

            _handler = new ClearCartCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldClearCart_WhenCalled()
        {
            _mockUserRepository.Setup(u => u.GetById(1)).Returns(new User { Id = 1 });
            _mockCartRepository.Setup(c => c.GetByUserId(1)).Returns(new List<CartItem> { new CartItem() });

            var result = await _handler.Handle(new ClearCartCommand { UserId = 1 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _mockCartRepository.Verify(c => c.Clear(1), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserIdIsInvalid()
        {
            _mockUserRepository.Setup(u => u.GetById(-1)).Returns((User?)null);

            var result = await _handler.Handle(new ClearCartCommand { UserId = -1 }, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}
