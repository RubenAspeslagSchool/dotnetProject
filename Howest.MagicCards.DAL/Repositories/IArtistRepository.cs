using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IArtistRepository
    {
        public IQueryable<Artist> GetAllArtists();
        public Artist? GetArtist(int id);
    }
}
