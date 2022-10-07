using WebApiUdemy.Models.Domain;

namespace WebApiUdemy.Repositories
{
    public interface IWalksRepository
    {
        Task<IEnumerable<Walk>> GetAllAsync();

        Task <Walk> GetAsync(Guid id);

        Task<Walk> AddWalkAsync(Walk walk);
        Task <Walk>DeleteAsync(Guid id);
        Task<Walk> UpdateAsync(Guid id, Walk walk);
    }
}
