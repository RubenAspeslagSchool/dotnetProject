using Howest.MagicCards.DAL.Json;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// todo: check if everything needs to be public
namespace Howest.MagicCards.DAL.Repositories
{
    public class JsonDeckRepository : IDeckRepository
    {
        public List<Deck> Decks { get; set; }

        public JsonDeckRepository()
        {
            Decks = getDecks();
        }

        public List<Deck> getDecks()
        {
            return new JsonRepository().getDecks().ToList();
        }

        public Deck getDeck(long id)
        {
            foreach (var deck in Decks)
            {
                if (deck.Id == id)
                {
                    return deck;
                }
            }
            throw new Exception("deck not fount");
        }

        public void saveDecks(List<Deck> decks)
        {
            new JsonRepository().SaveDecks(decks);
        }

        public void saveDeck(long id, Deck deck)
        { 
            new JsonRepository().SaveDeck(id, deck);
        }

        //todo:  get deck id function
        public void AddDeck(Deck deck)
        {
            deck.Id = Decks.Count > 0 ? Decks.Max(d => d.Id) + 1 : 1;
                       
            Decks.Add(deck);
            saveDecks(Decks);
        }

        public void RemoveDeck(long id)
        {
            Decks.Remove(getDeck(id));
            saveDecks(Decks);
        }

        public void Clear()
        {
            Decks = new List<Deck>();
            saveDecks(new List<Deck>());
        }

        public long CreateDeck(string name)
        {
            long id = GetDeckCount();
            Deck deck = new Deck() { Id = id, DeckName = name };
            this.AddDeck(deck);
            saveDeck(id, deck);
            return id;
        }

        public int GetDeckCount()
        {
            return Decks.Count();
        }

        public void AddCardToDeck(long deckId, long cardId)
        {
            Deck deck = getDeck(deckId);
            deck.AddCard(cardId);
            saveDeck(deckId, deck);
        }

        public bool RemoveCardFromDeck(long deckId, long cardId)
        {
            Deck deck = getDeck(deckId);
            bool found = deck.RemoveCard(cardId);
            saveDeck(deckId, deck);
            return found;
        }

        public void ClearDeck(long deckId)
        {
            Deck deck = getDeck(deckId);
            deck.Clear();
            saveDeck(deckId, deck);
        }

        public void ClearAllDecks()
        {
            Clear();
            saveDecks(new List<Deck>());
        }
        // todo : use linq
        public void UpdateCardsOfDeck(long id, Deck newDeck)
        {
            Deck deck = getDeck(id);
            //deck.Clear();
            foreach (CardDeck cardDeck in newDeck.CardDecks)
            {
                deck.AddCardDeck(cardDeck);
            }
            saveDecks(Decks);
        }

        public bool UpdateCardAmountInDeck(long deckId, long cardId, int amount)
        {
            Deck deck = getDeck(deckId);
            var cardDeck = deck.CardDecks.FirstOrDefault(cd => cd.CardId == cardId);
            if (cardDeck != null)
            {
                cardDeck.Amount = amount;
                saveDeck(deckId, deck);
                return true;
            }
            return false;
        }
    }
}

