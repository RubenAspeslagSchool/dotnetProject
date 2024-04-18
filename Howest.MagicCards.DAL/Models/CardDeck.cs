using System;
using System.Collections.Generic;

#nullable disable

namespace Howest.MagicCards.DAL.Models
{
    public partial class CardDeck
    {
        public long CardId { get; set; }
        public int DeckId { get; set; }
        public int Amount { get; set; }

        public virtual Card Card { get; set; }
        public virtual Deck Deck { get; set; }
    }
}
