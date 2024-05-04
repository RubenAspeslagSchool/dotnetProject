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
         IQueryable<Card> allCards = _db.Cards
            .Include(c => c.Artist)
            .Select(c => new
            {
                Card = c,
                Rarity = _db.Rarities.FirstOrDefault(r => r.Code == c.RarityCode),
                Set = _db.Sets.FirstOrDefault(s => s.Code == c.SetCode)
            })
            .Select(result => result.Card);
           return allCards;
        }

        public IQueryable<Card> GetAllCardsByArtistId(int id) 
        {
            return _db.Cards.Where(c => c.ArtistId == id).Select(c => c);
        }
    }
}
