using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlCardRepository : ICardRepository
    {
        private readonly MtgV1Context _db;
        public SqlCardRepository(MtgV1Context mtgContext)
        {
            _db = mtgContext;
        }

        public IQueryable<Card> GetAllCards()
        {
            return _db.Cards.Include(c => c.Artist);
        }

        public async Task<List<Card>> GetAllCardsAsync()
        {
            return await _db.Cards.Include(c => c.Artist).ToListAsync();
        }

        public async Task<List<Card>> GetAllCardsByArtistIdAsync(int id)
        {
            return await _db.Cards.Where(c => c.ArtistId == id).ToListAsync();
        }

        public async Task<Card> GetCardByIdAsync(long id)
        {
            return await _db.Cards
                .Include(c => c.Artist)
                .Include(c => c.CardColors)
                .Include(c => c.CardTypes)
                .Include(c => c.RarityCodeNavigation)
                .Include(c => c.SetCodeNavigation)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public List<Card> GetAllCardsByArtistId(long ArtistId)
        {
            return _db.Cards.Where(c => c.ArtistId == ArtistId).ToList();
        }

       
    }
}
