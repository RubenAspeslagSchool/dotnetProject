using System;
using System.Collections.Generic;

namespace Howest.MagicCards.DAL.Models
{
    public partial class CardDeck
    {
        public long CardId { get; set; }

        public long DeckId { get; set; }

        public int Amount { get; set; }   
    }
}
