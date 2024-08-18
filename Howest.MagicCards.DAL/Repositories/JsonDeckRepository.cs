using Howest.MagicCards.DAL.Json;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Howest.MagicCards.DAL.Exceptions;

namespace Howest.MagicCards.DAL.Repositories
{
    public class JsonDeckRepository : IDeckRepository
    {
        private readonly JsonSerialiser _jsonSerialiser;

        public List<Deck> Decks { get; set; }

        public JsonDeckRepository()
        {
            _jsonSerialiser = new JsonSerialiser();
            Decks = _jsonSerialiser.GetDecks().ToList();
        }

        public Deck GetDeck(long id)
        {
            Deck deck = Decks.FirstOrDefault(x => x.Id == id);
            if (deck == null)
            {
                throw new ArgumentNullException(nameof(deck), "Deck not found");
            }
            return deck;
        }

        private void SaveDecks()
        {
            _jsonSerialiser.SaveDecks(Decks);
        }

        private long GenerateNewId()
        {
            return Decks.Count > 0 ? Decks.Max(d => d.Id) + 1 : 1;
        }

        private void AddDeck(Deck deck)
        {
            Decks.Add(deck);
            SaveDecks();
        }

        public void RemoveDeck(long id)
        {
            Deck deck = GetDeck(id);
            if (deck == null)
            {
                throw new ArgumentNullException(nameof(deck), "Deck not found");
            }

            Decks.Remove(deck);
            SaveDecks();
        }

        public void Clear()
        {
            Decks.Clear();
            SaveDecks();
        }

        public long CreateDeck(string name)
        {
            long id = GenerateNewId();
            Deck deck = new Deck { Id = id, DeckName = name };
            AddDeck(deck);
            return id;
        }

        public void AddCardToDeck(long deckId, long cardId)
        {
            Deck deck = GetDeck(deckId);
            if (deck == null)
            {
                throw new ArgumentNullException(nameof(deck), "Deck not found");
            }
            
            if (GetCardCound(deck) > 60)
            {
                throw new ToManyCardsInDeckExeption();
                
            }
            deck.AddCard(cardId);
            SaveDecks();

        }

        private int GetCardCound(Deck deck)
        {
            int cardCound = 0;
            deck.CardDecks.ForEach(cardDeck => { cardCound = cardCound + cardDeck.Amount; });
            return cardCound;
        }

        public void RemoveCardFromDeck(long deckId, long cardId)
        {
            Deck deck = GetDeck(deckId);

            if (deck == null)
            {
                throw new ArgumentNullException(nameof(deck), "Deck not found");
            }

            deck.RemoveCard(cardId);
            SaveDecks();
        }


        public void ClearDeck(long deckId)
        {
            Deck deck = GetDeck(deckId);

            if (deck == null)
            {
                throw new ArgumentNullException(nameof(deck), "Deck not found");
            }

            deck.Clear();
            SaveDecks();
        }


        public void ClearAllDecks()
        {
            Clear();
        }

        public void UpdateCardsOfDeck(long id, Deck newDeck)
        {
            Deck deck = GetDeck(id);

            if (deck == null)
            {
                throw new ArgumentNullException(nameof(deck), "Deck not found");
            }

            deck.CardDecks.Clear();
            newDeck.CardDecks.ForEach(deck.AddCardDeck);
            SaveDecks();
        }


        public void UpdateCardAmountInDeck(long deckId, long cardId, int amount)
        {
            Deck deck = GetDeck(deckId);

            if (deck == null)
            {
                throw new ArgumentNullException(nameof(deck), "Deck not found");
            }

            CardDeck cardDeck = deck.CardDecks.FirstOrDefault(cd => cd.CardId == cardId);

            if (cardDeck == null)
            {
                throw new ArgumentNullException(nameof(cardDeck), "Card not found in deck");
            }

            cardDeck.Amount = amount;
            SaveDecks();
        }

        public void UpdateDeckName(long deckId, string newDeckName)
        {
            Deck deck = GetDeck(deckId);

            if (deck == null)
            {
                throw new ArgumentNullException(nameof(deck), "Deck not found");
            }

            deck.DeckName = newDeckName;
            SaveDecks();
        }

    }
}
