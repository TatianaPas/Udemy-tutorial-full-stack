using Microsoft.EntityFrameworkCore;
using WebApiUdemy.Data;
using WebApiUdemy.Models.Domain;

namespace WebApiUdemy.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddWalkDifficultyAsync(WalkDifficulty newWalkDifficulty)
        {
            newWalkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(newWalkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return newWalkDifficulty;

        }

        public async Task<WalkDifficulty> DeleteWalkDifficultyAsync(Guid id)
        {
            var walkDif = nZWalksDbContext.WalkDifficulties.FirstOrDefault(x => x.Id == id);
            if(walkDif==null)
            {
                return null;
            }
            nZWalksDbContext.WalkDifficulties.Remove(walkDif);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDif;

        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulties.FirstOrDefaultAsync(x =>x.Id == id);
        }

        public async Task<IEnumerable<WalkDifficulty>> GetWalkDifficultyListAsync()
        {
            return await nZWalksDbContext.WalkDifficulties.ToListAsync();
        }

        public async Task<WalkDifficulty> UpdateWalkDifficultyAsync(Guid id, WalkDifficulty newWalkDifficulty)
        {
            var ExwalkDifficulty = await nZWalksDbContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
            if(ExwalkDifficulty == null)
            {
                return null;
            }
            ExwalkDifficulty.Code = newWalkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return ExwalkDifficulty;

        }
    }
}
