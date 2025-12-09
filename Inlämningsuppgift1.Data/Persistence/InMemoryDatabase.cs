using Inlämningsuppgift_1.Data.Entities;

namespace Inlämningsuppgift_1.Data.Persistence
{
    public class InMemoryDatabase
    {
        public List<User> Users { get; set; }
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
        public Dictionary<int, List<CartItem>> Carts { get; set; }
        public Dictionary<string, int> Tokens { get; set; }


        private int _nextProductId = 4;
        private int _nextOrderId = 1;
        private int _nextUserId = 3;

        public InMemoryDatabase()
        {
            Products = new List<Product>
            {
                new Product { Id = 1, Name = "Pen", Price = 1.5m, Stock = 100 },
                new Product { Id = 2, Name = "Notebook", Price = 3.0m, Stock = 50 },
                new Product { Id = 3, Name = "Mug", Price = 6.0m, Stock = 20 }
              };

            Users = new List<User>
            {
              new User { Id = 1, Username = "alice", Password = "password", Email = "alice@example.com" },
              new User { Id = 2, Username = "bob", Password = "password", Email = "bob@example.com" }
            };

            Orders = new List<Order>();

            Carts = new Dictionary<int, List<CartItem>>();

            Tokens = new Dictionary<string, int>();
        }

        public int GetNextProductId() => _nextProductId++;
        public int GetNextOrderId() => _nextOrderId++;
        public int GetNextUserId() => _nextUserId++;
    }
}
