using Inlämningsuppgift_1.Core.Commands.Carts;
using Inlämningsuppgift_1.Core.Handlers.Commands.Carts;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Commands.Carts
{
    public class RemoveFromCartHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly RemoveFromCartCommandHandler _handler;

        public RemoveFromCartHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCartRepository = new Mock<ICartRepository>();

            _mockUnitOfWork.Setup(u => u.Carts).Returns(_mockCartRepository.Object);

            _handler = new RemoveFromCartCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldRemoveCartForUser()
        {
            var userId = 1;
            var productId = 2;
            var cartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2 },
                new CartItem { ProductId = 2, Quantity = 1 }
            };

            _mockCartRepository.Setup(c => c.GetByUserId(userId))
                .Returns(cartItems);

            var command = new RemoveFromCartCommand { UserId = userId, ProductId = productId };

            var result = await _handler.Handle(command, CancellationToken.None);

            _mockCartRepository.Verify(c => c.Add(userId, It.Is<List<CartItem>>(list => list.Count == 1)), Times.Once);
            Assert.True(result.IsSuccess);
        }

    }
}
