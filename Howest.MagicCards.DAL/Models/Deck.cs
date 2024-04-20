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

        public virtual ICollection<CardDeck> CardDecks { get; set; }

        internal void AddCard(Card card)
        {
            bool found = false;
            foreach (var deckCard in CardDecks) 
            {
                if (deckCard.CardId == card.Id)
                {
                    found = true;
                    deckCard.Amount++;
                }   
            }

            if (!found)
            {
                CardDecks.Add(new CardDeck() {
                    CardId = card.Id,
                    DeckId = this.Id,
                    Amount = 1
                });
            }
        }
    }
}
