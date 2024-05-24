using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckRepository
    {
        List<Deck> Decks { get; set; }

        void AddCardToDeck(long deckId, long cardId);
        void Clear();
        void ClearAllDecks();
        void ClearDeck(long deckId);
        long CreateDeck(string name);
        Deck GetDeck(long id);
        void RemoveCardFromDeck(long deckId, long cardId);
        void RemoveDeck(long id);
        void UpdateCardAmountInDeck(long deckId, long cardId, int amount);
        void UpdateCardsOfDeck(long id, Deck newDeck);
        void UpdateDeckName(long deckId, string newDeckName);
    }
}