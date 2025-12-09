using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Persistence;

namespace Inlämningsuppgift1.Tests.TestHelpers.Fakes
{
    public class FakeInMemoryDatabase : InMemoryDatabase
    {
        public FakeInMemoryDatabase()
        {
            Reset();
        }

         public void Reset()
        {
            Products = new List<Product>();
            Users = new List<User>();
            Orders = new List<Order>();
            Carts = new Dictionary<int, List<CartItem>>();
            Tokens = new Dictionary<string, int>();
        }

        public void SeedTestData()
        {
            Products.Add(new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10.00m,
                Stock = 100
            });

            Users.Add(new User
            {
                Id = 1,
                Username = "testuser",
                Password = "hashed_password",
                Email = "test@test.com"
            });
        }
    }
}
