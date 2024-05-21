using Howest.MagicCards.DAL.Json;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Howest.MagicCards.DAL.Repositories
{
    public class JsonDeckRepository : IDeckRepository
    {
        private JsonSerialiser _jsonSerialiser = new JsonSerialiser();

        public List<Deck> Decks { get; set; }

        public JsonDeckRepository()
        {
            Decks = GetDecks();
        }

        public List<Deck> GetDecks()
        {
            return new JsonSerialiser().GetDecks().ToList();
        }

        public Deck GetDeck(long id)
        {
            Deck deck = null;
            Decks.ForEach(currentDeck =>
            {
                if (currentDeck.Id == id)
                {
                    deck = currentDeck;
                }
            });

            return deck ?? throw new Exception("Deck not found");
        }

        private void SaveDecks(List<Deck> decks)
        {
            _jsonSerialiser.SaveDecks(decks);
        }

        private long GenerateNewId()
        {
            return Decks.Count > 0 ? Decks.Max(d => d.Id) + 1 : 1;
        }

        private void AddDeck(Deck deck)
        {
            Decks.Add(deck);
            SaveDecks(Decks);
        }

        public void RemoveDeck(long id)
        {
            Decks.Remove(GetDeck(id));
            SaveDecks(Decks);
        }

        public void Clear()
        {
            Decks = new List<Deck>();
            SaveDecks(new List<Deck>());
        }

        public long CreateDeck(string name)
        {
            long id = GenerateNewId();
            Deck deck = new Deck() { Id = id, DeckName = name };
            this.AddDeck(deck);
            SaveDeck(id, deck);
            return id;
        }

        public void AddCardToDeck(long deckId, long cardId)
        {
            Deck deck = GetDeck(deckId);
            deck.AddCard(cardId);
            SaveDeck(deckId, deck);
        }

        public void RemoveCardFromDeck(long deckId, long cardId)
        {
            Deck deck = GetDeck(deckId);
            deck.RemoveCard(cardId);
            SaveDeck(deckId, deck);
        }

        public void ClearDeck(long deckId)
        {
            Deck deck = GetDeck(deckId);
            deck.Clear();
            SaveDeck(deckId, deck);
        }

        public void ClearAllDecks()
        {
            Clear();
            SaveDecks(new List<Deck>());
        }

        public void UpdateCardsOfDeck(long id, Deck newDeck)
        {
            Deck deck = GetDeck(id);
            //deck.Clear();

            newDeck.CardDecks.ForEach(cardDeck =>
            {
                deck.AddCardDeck(cardDeck);
            });
            SaveDecks(Decks);
        }

        public void UpdateCardAmountInDeck(long deckId, long cardId, int amount)
        {
            Deck deck = GetDeck(deckId);
            var cardDeck = deck.CardDecks.FirstOrDefault(cd => cd.CardId == cardId);
            if (cardDeck != null)
            {
                cardDeck.Amount = amount;
                SaveDeck(deckId, deck);
            }
            throw new Exception("Deck not found");
        }

        public void UbdateDeckName(long deckId, String newDeckName)
        {
            Deck deck = GetDeck(deckId);
            deck.DeckName = newDeckName;
            SaveDeck(deckId, deck);
        }

        private void SaveDeck(long deckId, Deck newDeck)
        {
            List<Deck> decks = GetDecks().ToList();
            Deck oldDeck = decks.FindLast(deck => deck.Id == deckId);
            if (oldDeck is Deck && oldDeck is not null)
            {
                oldDeck.DeckName = newDeck.DeckName;
                oldDeck.CardDecks = newDeck.CardDecks;
            }
            else
            {
                decks.Add(newDeck);
            }
            _jsonSerialiser.SaveDecks(decks);
        }
    }
}
