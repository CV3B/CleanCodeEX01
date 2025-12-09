namespace Inlämningsuppgift_1.Data.Interfaces
{
    public interface ITokenRepository
    {
        void Add(int userId, string token);
        int? GetUserIdByToken(string token);
        void Remove(string token);
    }
}