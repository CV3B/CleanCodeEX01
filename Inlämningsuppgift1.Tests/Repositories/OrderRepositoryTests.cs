using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Repositories;
using Inlämningsuppgift1.Tests.TestHelpers.Fakes;

namespace Inlämningsuppgift1.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly FakeInMemoryDatabase _fakeDatabase;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            _fakeDatabase = new FakeInMemoryDatabase();
            _repository = new OrderRepository(_fakeDatabase);
        }

        [Fact]
        public void Add_ValidOrder_OrderIsAddedToDatabase()
        {
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                CreatedAt = DateTime.UtcNow,
                Total = 100m,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, ProductName = "Product 1", Quantity = 2, Price = 50m }
                }
            };

            _repository.Add(order);

            Assert.Single(_fakeDatabase.Orders);
            Assert.Equal(1, _fakeDatabase.Orders[0].Id);
            Assert.Equal(100m, _fakeDatabase.Orders[0].Total);
        }

        [Fact]
        public void Add_MultipleOrders_AllOrdersAreAdded()
        {
            var order1 = new Order { Id = 1, UserId = 1, Total = 100m, OrderItems = new List<OrderItem>() };
            var order2 = new Order { Id = 2, UserId = 1, Total = 200m, OrderItems = new List<OrderItem>() };
            var order3 = new Order { Id = 3, UserId = 2, Total = 300m, OrderItems = new List<OrderItem>() };

            _repository.Add(order1);
            _repository.Add(order2);
            _repository.Add(order3);

            Assert.Equal(3, _fakeDatabase.Orders.Count);
        }

        [Fact]
        public void GetByOrderId_NonExistingOrder_ReturnsNull()
        {
            var result = _repository.GetByOrderId(999);
            Assert.Null(result);
        }

        [Fact]
        public void GetByUserId_ExistingOrders_ReturnsAllUserOrders()
        {
            var order1 = new Order { Id = 1, UserId = 1, Total = 100m, OrderItems = new List<OrderItem>() };
            var order2 = new Order { Id = 2, UserId = 1, Total = 200m, OrderItems = new List<OrderItem>() };
            var order3 = new Order { Id = 3, UserId = 2, Total = 300m, OrderItems = new List<OrderItem>() };

            _fakeDatabase.Orders.Add(order1);
            _fakeDatabase.Orders.Add(order2);
            _fakeDatabase.Orders.Add(order3);

            var result = _repository.GetByUserId(1);

            Assert.NotNull(result);
            Assert.Equal(2, result!.Count);
            Assert.All(result, o => Assert.Equal(1, o.UserId));
        }

        [Fact]
        public void GetByUserId_NoOrders_ReturnsEmptyList()
        {
            var result = _repository.GetByUserId(999);
            Assert.NotNull(result);
            Assert.Empty(result!);
        }

        [Fact]
        public void GetByUserId_OnlyReturnsOrdersForSpecificUser()
        {
            var order1 = new Order { Id = 1, UserId = 1, Total = 100m, OrderItems = new List<OrderItem>() };
            var order2 = new Order { Id = 2, UserId = 2, Total = 200m, OrderItems = new List<OrderItem>() };
            var order3 = new Order { Id = 3, UserId = 3, Total = 300m, OrderItems = new List<OrderItem>() };

            _fakeDatabase.Orders.Add(order1);
            _fakeDatabase.Orders.Add(order2);
            _fakeDatabase.Orders.Add(order3);

            var result = _repository.GetByUserId(2);

            Assert.NotNull(result);
            Assert.Single(result!);
            Assert.Equal(2, result[0].Id);
            Assert.Equal(2, result[0].UserId);
        }
    }
}
