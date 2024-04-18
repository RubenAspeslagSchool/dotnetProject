using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlDeckRepository : IDeckRepository
    {
        private readonly MtgContext _db;
        public SqlDeckRepository(MtgContext mtgContext)
        {
            _db = mtgContext;
        }

        public IQueryable<Deck> GetAllDecks()
        {
            return _db.Decks
                        .Include(d => d.CardDecks)
                        .ThenInclude(cd => cd.Card);
        }

        public Deck AddDeck(Deck deck)
        {
            _db.Decks.Add(deck);
            _db.SaveChanges();

            return deck;
        }

        public Deck? RemoveDeck(int id)
        {
            Deck? deckToRemove = _db.Decks.Include(d => d.CardDecks).FirstOrDefault(d => d.Id == id);

            if(deckToRemove != null)
            {
                deckToRemove.CardDecks.ToList().ForEach(d => _db.CardDecks.Remove(d));
                _db.Remove(deckToRemove);
                _db.SaveChanges();
            }

            return deckToRemove;
        }
    }
}
