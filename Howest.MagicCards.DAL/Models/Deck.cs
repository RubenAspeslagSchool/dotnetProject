using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

#nullable disable

namespace Howest.MagicCards.DAL.Models
{
    public partial class Deck
    {
        public Deck()
        {
            CardDecks = new HashSet<CardDeck>();
        }

        public long Id { get; set; }
        public string DeckName { get; set; }

        public virtual ICollection<CardDeck> CardDecks { get; set; }

    }
}
