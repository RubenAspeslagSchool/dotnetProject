using System;
using System.Collections.Generic;

#nullable disable

namespace Howest.MagicCards.DAL.Models
{
    public partial class Deck
    {
        public Deck()
        {
            CardDecks = new HashSet<CardDeck>();
        }

        public int Id { get; set; }
        public string DeckName { get; set; }

        public virtual ICollection<int> CardIds { get; set; }
    }
}
