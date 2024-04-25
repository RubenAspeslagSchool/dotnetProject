﻿using Howest.MagicCards.DAL.Json;
using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return new JsonRepo().getDecks().ToList();
        }

        public Deck getDeck(int id)
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
            new JsonRepo().save(decks);
        }

        public void saveDeck(int id, Deck deck)
        { 
            new JsonRepo().save(id, deck);
        }

        public void AddDeck(Deck deck)
        {
            deck.Id = Decks.Count > 0 ? Decks.Max(d => d.Id) + 1 : 1;
                       
            Decks.Add(deck);
            saveDecks(Decks);
        }

        public void RemoveDeck(int id)
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
            int id = getDeckCount();
            Deck deck = new Deck() { Id = id, DeckName = name };
            this.AddDeck(deck);
            saveDeck(id, deck);
            return id;
        }

        public int getDeckCount()
        {
            return Decks.Count();
        }

        public void AddCardToDeck(int deckId, long cardId)
        {
            Deck deck = getDeck(deckId);
            deck.AddCard(cardId);
            saveDeck(deckId, deck);
        }

        public bool RemoveCardFromDeck(int deckId, long cardId)
        {
            Deck deck = getDeck(deckId);
            bool found = deck.RemoveCard(cardId);
            saveDeck(deckId, deck);
            return found;
        }

        public void ClearDeck(int deckId)
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

        public void UpdateCardsOfDeck(int id, Deck newDeck)
        {
            Deck deck = getDeck(id);
            //deck.Clear();
            foreach (CardDeck cardDeck in newDeck.CardDecks)
            {
                deck.AddCardDeck(cardDeck);
            }
            saveDecks(Decks);
        }

        public bool UpdateCardAmountInDeck(int deckId, long cardId, int amount)
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
