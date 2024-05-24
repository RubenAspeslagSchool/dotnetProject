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
            CardDeck? cardDeck = deck.CardDecks.FirstOrDefault(d => d.CardId == cardId);
            if (cardDeck is not null)
            {
                cardDeck.Amount++;
            } 
            else
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

        public static void RemoveCard(this Deck deck, long cardId)
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

            if (!found) { throw new Exception("Card not found"); }
        }
    }
}