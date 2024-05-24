using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlRarityRepository : IRarityRepository
    {
        private readonly MtgV1Context _db;
        public SqlRarityRepository(MtgV1Context mtgContext)
        {
            _db = mtgContext;
        }

        public async Task<List<Rarity>> GetAllRaritiesAsync()
        {
            return await _db.Rarities.ToListAsync();
        }

        public async Task<Rarity> GetRarityAsync(string code)
        {
            return await _db.Rarities.FirstOrDefaultAsync(r => r.Code == code);
        }
    }
}
