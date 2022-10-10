using WebApiUdemy.Models.Domain;

namespace WebApiUdemy.Repositories
{
    public interface IUserRepository
    {
        Task <UserNZ> AuthenticateAsync(string username, string password);
    }
}
