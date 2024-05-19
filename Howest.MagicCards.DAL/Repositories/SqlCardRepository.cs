using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


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
            IQueryable<Card> allCards = _db.Cards.Include(c => c.Artist);
           return allCards;
        }

        public IQueryable<Card> GetAllCardsByArtistId(int id) 
        {
            return _db.Cards.Where(c => c.ArtistId == id).Select(c => c);
        }

        public Card GetCardById(long id)
        {
            return _db.Cards
                .Include(c => c.Artist)
                .Include(c => c.CardColors)
                .Include(c => c.CardTypes)
                .Include(c => c.RarityCodeNavigation)
                .Include(c => c.SetCodeNavigation)
                .FirstOrDefault(c => c.Id == id);
        }

        public async Task<IQueryable<Card>> GetAllCardsAsync()
        {
            return await Task.FromResult(_db.Cards.AsQueryable());
        }

        public async Task<Card> GetCardByIdAsync(long id)
        {
            return await _db.Cards.FindAsync(id);
        }
    }
}
