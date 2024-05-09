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
        List<Deck> getDecks();
        void RemoveCardFromDeck(long deckId, long cardId);
        void RemoveDeck(long id);
        void UbdateDeckName(long deckId, string newDeckName);
        bool UpdateCardAmountInDeck(long deckId, long cardId, int amount);
        void UpdateCardsOfDeck(long id, Deck newDeck);
    }
}