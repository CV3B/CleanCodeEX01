using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Repositories;
using Inlämningsuppgift1.Tests.TestHelpers.Fakes;
using Xunit;

namespace Inlämningsuppgift1.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly FakeInMemoryDatabase _fakeDatabase;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _fakeDatabase = new FakeInMemoryDatabase();
            _repository = new UserRepository(_fakeDatabase);
        }

        [Fact]
        public void Add_ValidUser_UserIsAddedWithAutoIncrementedId()
        {
            var user = new User
            {
                Username = "testuser",
                Password = "hashed_password",
                Email = "test@example.com"
            };

            _repository.Add(user);

            Assert.Single(_fakeDatabase.Users);
            Assert.True(user.Id > 0, "User ID should be auto-assigned to a positive value");
            Assert.Equal("testuser", _fakeDatabase.Users[0].Username);
        }

        [Fact]
        public void Add_MultipleUsers_AssignsIncrementalIds()
        {
            var user1 = new User { Username = "user1", Password = "pass1", Email = "user1@test.com" };
            var user2 = new User { Username = "user2", Password = "pass2", Email = "user2@test.com" };
            var user3 = new User { Username = "user3", Password = "pass3", Email = "user3@test.com" };

            _repository.Add(user1);
            _repository.Add(user2);
            _repository.Add(user3);

            Assert.Equal(3, _fakeDatabase.Users.Count);
            Assert.True(user1.Id > 0, "User1 ID should be positive");
            Assert.Equal(user1.Id + 1, user2.Id);
            Assert.Equal(user2.Id + 1, user3.Id);
        }

        [Fact]
        public void GetById_ExistingUser_ReturnsUser()
        {
            var user = new User { Id = 1, Username = "testuser", Password = "hashed_password", Email = "test@example.com" };
            _fakeDatabase.Users.Add(user);

            var result = _repository.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public void GetById_NonExistingUser_ReturnsNull()
        {
            var result = _repository.GetById(999);
            Assert.Null(result);
        }

        [Fact]
        public void GetByUsername_ExistingUser_ReturnsUser()
        {
            var user = new User { Id = 1, Username = "testuser", Password = "hashed_password", Email = "test@example.com" };
            _fakeDatabase.Users.Add(user);

            var result = _repository.GetByUsername("testuser");

            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public void GetByUsername_NonExistingUser_ReturnsNull()
        {
            var result = _repository.GetByUsername("nonexistent");
            Assert.Null(result);
        }

        [Fact]
        public void GetByUsername_CaseInsensitive_ReturnsUser()
        {
            var user = new User { Id = 1, Username = "TestUser", Password = "hashed_password", Email = "test@example.com" };
            _fakeDatabase.Users.Add(user);

            var result1 = _repository.GetByUsername("testuser");
            var result2 = _repository.GetByUsername("TESTUSER");
            var result3 = _repository.GetByUsername("TestUser");

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            Assert.Equal("TestUser", result1!.Username);
        }
    }
}
