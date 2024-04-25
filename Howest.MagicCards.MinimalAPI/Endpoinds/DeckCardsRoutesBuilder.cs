using Howest.MagicCards.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.MinimalAPI.Endpoinds
{
    public static class DeckCardsRoutesBuilder
    {
        public static RouteGroupBuilder MapCardDeckApi(this RouteGroupBuilder group)
        {
            // Update a card within a deck by deck ID and card ID
            group.MapPut("/{cardId}", (IDeckRepository repository, int deckId, long cardId, [FromBody] int amount) =>
            {
                var deck = repository.getDeck(deckId);
                if (deck == null)
                    return Results.NotFound("Deck not found");

                bool cardUpdated = repository.UpdateCardAmountInDeck(deckId, cardId, amount);
                if (!cardUpdated)
                    return Results.NotFound("Card not found in deck");

                return Results.Ok("Card updated successfully");
            });

            // Delete a card from a deck by deck ID and card ID
            group.MapDelete("/{cardId}", (IDeckRepository repository, int deckId, long cardId) =>
            {
                var deck = repository.getDeck(deckId);
                if (deck == null)
                    return Results.NotFound("Deck not found");

                bool cardRemoved = repository.RemoveCardFromDeck(deckId, cardId);
                if (!cardRemoved)
                    return Results.NotFound("Card not found in deck");

                return Results.Ok("Card removed from deck successfully");
            });
            return group;
        }
    }
}
