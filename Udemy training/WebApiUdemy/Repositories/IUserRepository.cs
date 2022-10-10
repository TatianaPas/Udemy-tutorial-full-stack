using WebApiUdemy.Models.Domain;

namespace WebApiUdemy.Repositories
{
    public interface IUserRepository
    {
        Task <User> AuthenticateAsync(string username, string password);
    }
}
