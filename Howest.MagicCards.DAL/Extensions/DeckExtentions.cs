using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Extensions
{
    public static class DeckExtentions
    {
        public static void AddCard(this Deck deck, long cardId)
        {
            bool found = false;
            deck.CardDecks.ForEach(deckCard => 
            {
                
            });
            foreach (var deckCard in deck.CardDecks)
            {
                if (deckCard.CardId == cardId)
                {
                    found = true;
                    deckCard.Amount++;
                }
            }

            if (!found)
            {
                deck.CardDecks.Add(new CardDeck()
                {
                    CardId = cardId,
                    DeckId = deck.Id,
                    Amount = 1
                });
            }
        }

        public static void AddCardDeck(this Deck deck, CardDeck cardDeck)
        {
            deck.CardDecks.Add(cardDeck);
        }

        public static void Clear(this Deck deck)
        {
            deck.CardDecks?.Clear();
        }

        public static Boolean RemoveCard(this Deck deck, long cardId)
        {
            bool found = false;
            deck.CardDecks.ForEach(deckCard =>
            {
                if (deckCard.CardId == cardId)
                {
                    found = true;
                    deckCard.Amount--;
                }
            });

            return found;
        }
    }
}
