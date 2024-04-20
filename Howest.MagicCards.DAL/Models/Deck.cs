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

        public int Id { get; set; }
        public string DeckName { get; set; }

        public virtual ICollection<CardDeck> CardDecks { get; set; }

        internal void AddCard(long cardId)
        {
            bool found = false;
            foreach (var deckCard in CardDecks) 
            {
                if (deckCard.CardId == cardId)
                {
                    found = true;
                    deckCard.Amount++;
                }   
            }

            if (!found)
            {
                CardDecks.Add(new CardDeck() {
                    CardId = cardId,
                    DeckId = this.Id,
                    Amount = 1
                });
            }
        }

        internal void AddCardDeck(CardDeck cardDeck)
        {
            CardDecks.Add(cardDeck);
        }

        internal void Clear()
        {
            CardDecks?.Clear();
        }

        internal Boolean RemoveCard(long cardId)
        {
            bool found = false;
            foreach (var deckCard in CardDecks)
            {
                if (deckCard.CardId == cardId)
                {
                    found = true;
                    deckCard.Amount--;
                }
            }
            return found;
        }
    }
}
