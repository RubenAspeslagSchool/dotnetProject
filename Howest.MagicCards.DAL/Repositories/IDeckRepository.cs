using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckRepository
    {
        public IQueryable<Deck> GetAllDecks();
        public Deck AddDeck(Deck deck);

        public Deck? RemoveDeck(int id);
    }
}
