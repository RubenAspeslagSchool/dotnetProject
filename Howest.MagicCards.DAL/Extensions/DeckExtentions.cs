﻿using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
            CardDeck cardDeck = deck.CardDecks.FirstOrDefault(CardDeck => CardDeck.CardId == cardId);
            if (cardDeck is not null) 
            {
                cardDeck.Amount--;
                if (cardDeck.Amount <= 0)
                {
                    deck.CardDecks.Remove(cardDeck);
                }
            } 
            else
            { 
                throw new ArgumentNullException("Card not found"); 
            }
        }
    }
}