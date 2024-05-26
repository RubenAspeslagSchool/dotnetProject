using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlArtistRepository : IArtistRepository
    {
        private readonly MtgV1Context _db;
        public SqlArtistRepository(MtgV1Context mtgContext)
        {
            _db = mtgContext;
        }

        public async Task<List<Artist>> GetAllArtistsAsync()
        {
            return await _db.Artists.ToListAsync();
        }

        public async Task<Artist?> GetArtistAsync(int id)
        {
            return await _db.Artists.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
