using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Persistence;
using Inlämningsuppgift_1.Data.Repositories;

namespace Inlämningsuppgift1.Tests.Repositories
{
    public class CartRepositoryTests
    {
        private InMemoryDatabase _db;
        private CartRepository _repository;

        public CartRepositoryTests()
        {
            _db = new InMemoryDatabase();
            _repository = new CartRepository(_db);
        }

        [Fact]
        public void Add_Cart_AddsCartToDatabase()
        {
            var cartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2 },
                new CartItem { ProductId = 2, Quantity = 5}
            };

            _repository.Add(1, cartItems);

            var retrievedCart = _repository.GetByUserId(1);

            Assert.NotNull(retrievedCart);
            Assert.Equal(2, retrievedCart.Count);
        }

        [Fact]
        public void GetByUserId_ExistingUserId_ReturnsCartItems()
        {
            var cartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2 },
                new CartItem { ProductId = 2, Quantity = 5 }
            };
            _db.Carts.Add(1, cartItems);

            var retrievedCart = _repository.GetByUserId(1);

            Assert.NotNull(retrievedCart);
            Assert.Equal(2, retrievedCart.Count);
        }

        [Fact]
        public void GetByUserId_NonExistingUserId_ReturnsEmptyList()
        {
            var retrievedCart = _repository.GetByUserId(999);

            Assert.NotNull(retrievedCart);
            Assert.Empty(retrievedCart);
        }

        [Fact]
        public void Remove_ExistingCart_RemovesCartFromDatabase()
        {
            var cartItems = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 2 } };
            _db.Carts.Add(1, cartItems);

            _repository.Remove(1);

            var retrievedCart = _repository.GetByUserId(1);
            Assert.Empty(retrievedCart);
        }

        [Fact]
        public void Clear_ExistingCart_ClearsAllItems()
        {
            var cartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2 },
                new CartItem { ProductId = 2, Quantity = 5 }
            };
            _db.Carts.Add(1, cartItems);

            _repository.Clear(1);

            var retrievedCart = _repository.GetByUserId(1);
            Assert.NotNull(retrievedCart);
            Assert.Empty(retrievedCart);
        }

    }

}
