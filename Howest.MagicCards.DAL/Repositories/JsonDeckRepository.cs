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
        private readonly JsonSerialiser _jsonSeriliser;

        public List<Deck> Decks { get; set; }

        public JsonDeckRepository()
        {
            _jsonSeriliser = new JsonSerialiser();
            Decks = _jsonSeriliser.GetDecks().ToList();
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
            _jsonSeriliser.SaveDecks(Decks);
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
            if (deck != null )
            {
                if (GetCardCound(deck) <= 60)
                {
                    deck.AddCard(cardId);
                    SaveDecks();
                } else
                {
                    throw new ToManyCardsInDeckExeption();
                }
            } else
            {
                throw new ArgumentNullException(nameof(deck), "Deck not found");
            }
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
            if (deck != null)
            {
                deck.RemoveCard(cardId);
                SaveDecks();
            }
        }

        public void ClearDeck(long deckId)
        {
            Deck deck = GetDeck(deckId);
            if (deck != null)
            {
                deck.Clear();
                SaveDecks();
            }
        }

        public void ClearAllDecks()
        {
            Clear();
        }

        public void UpdateCardsOfDeck(long id, Deck newDeck)
        {
            Deck deck = GetDeck(id);
            if (deck != null)
            {
                deck.CardDecks.Clear();
                newDeck.CardDecks.ForEach(deck.AddCardDeck);
                SaveDecks();
            }
        }

        public void UpdateCardAmountInDeck(long deckId, long cardId, int amount)
        {
            Deck deck = GetDeck(deckId);
            if (deck != null)
            {
                CardDeck cardDeck = deck.CardDecks.FirstOrDefault(cd => cd.CardId == cardId);
                if (cardDeck != null)
                {
                    cardDeck.Amount = amount;
                    SaveDecks();
                }
                else
                {
                    throw new ArgumentNullException(nameof(Card), "Card not found in deck");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(deck), "Deck not found");
            }
        }

        public void UpdateDeckName(long deckId, string newDeckName)
        {
            Deck deck = GetDeck(deckId);
            if (deck is not null)
            {
                deck.DeckName = newDeckName;
                SaveDecks();
            } else 
            { 
                throw new ArgumentNullException(nameof(deck), "deck not fount"); 
            }
        }
    }
}
