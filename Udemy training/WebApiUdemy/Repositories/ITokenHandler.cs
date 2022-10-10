using WebApiUdemy.Models.Domain;

namespace WebApiUdemy.Repositories
{
    public interface ITokenHandler
    {
        Task<string> CreatTokenAsync(User user);
    }
}
