using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckRepository
    {
        List<Deck> Decks { get; set; }

        void AddCardToDeck(long deckId, long cardId);
        void AddDeck(Deck deck);
        void Clear();
        void ClearAllDecks();
        void ClearDeck(long deckId);
        long CreateDeck(string name);
        Deck getDeck(long id);
        int GetDeckCount();
        List<Deck> getDecks();
        bool RemoveCardFromDeck(long deckId, long cardId);
        void RemoveDeck(long id);
        void saveDeck(long id, Deck deck);
        void saveDecks(List<Deck> decks);
        void UpdateCardsOfDeck(long id, Deck newDeck);
        bool UpdateCardAmountInDeck(long deckId, long cardId, int amount);
    }
}