using WebApiUdemy.Models.Domain;

namespace WebApiUdemy.Repositories
{
    public interface IWalkDifficultyRepository
    {
        Task<WalkDifficulty> GetAsync(Guid id);
        Task<IEnumerable<WalkDifficulty>> GetWalkDifficultyListAsync();
        Task<WalkDifficulty> AddWalkDifficultyAsync(WalkDifficulty newWalkDifficulty);
        Task<WalkDifficulty>UpdateWalkDifficultyAsync(Guid id, WalkDifficulty newWalkDifficulty);
        Task<WalkDifficulty>DeleteWalkDifficultyAsync(Guid id);
    }
}
