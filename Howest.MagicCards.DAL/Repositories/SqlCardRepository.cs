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
            IQueryable<Card> allCards = _db.Cards; 
                                            //.Include(c => c.Rarity)
                                            //.Include(c => c.Artist)
                                            //.Include(c => c.Set)
                                            //.Select(c => c);
            return allCards;
        }

        public IQueryable<Card> GetAllCardsByArtistId(int id) 
        {
            return _db.Cards.Where(c => c.ArtistId == id).Select(c => c);
        }
    }
}
