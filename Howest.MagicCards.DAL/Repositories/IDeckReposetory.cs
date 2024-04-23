using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckReposetory
    {
        List<Deck> Decks { get; set; }

        void AddCardToDeck(int deckId, long cardId);
        void AddDeck(Deck deck);
        void Clear();
        void ClearAllDecks();
        void ClearDeck(int deckId);
        long CreateDeck(string name);
        Deck getDeck(int id);
        int getDeckCount();
        List<Deck> getDecks();
        bool RemoveCardFromDeck(int deckId, long cardId);
        void RemoveDeck(int id);
        void saveDeck(int id, Deck deck);
        void saveDecks(List<Deck> decks);
        void UbdateCardsOfDeck(int id, Deck newDeck);
        bool UpdateCardAmountInDeck(int deckId, long cardId, int amount);
    }
}