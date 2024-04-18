using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlArtistRepository : IArtistRepository
    {
        private readonly MtgContext _db;
        public SqlArtistRepository(MtgContext mtgContext)
        {
            _db = mtgContext;
        }

        public IQueryable<Artist> GetAllArtists()
        {
            return _db.Artists.Select(a => a);
        }

        public Artist? GetArtist(int id)
        {
            Artist? artist = _db.Artists.FirstOrDefault(a => a.Id == id);
            return artist;
        }
    }
}
