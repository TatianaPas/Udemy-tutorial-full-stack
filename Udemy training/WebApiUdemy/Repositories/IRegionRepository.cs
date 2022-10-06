using WebApiUdemy.Models.Domain;

namespace WebApiUdemy.Repositories
{
    public interface IRegionRepository
    {
       Task<IEnumerable<Region>> GetAllAsync();
    }
}
