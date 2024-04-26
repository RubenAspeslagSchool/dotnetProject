using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.MinimalAPI.Endpoints
{
    public static class DeckCardsRoutesBuilder
    {
        public static RouteGroupBuilder MapCardDeckApi(this RouteGroupBuilder group)
        {
            group.MapGet("/", (IMapper mapper,  IDeckRepository repository, long deckId) =>
            {
                var deck = repository.getDeck(deckId);
                if (deck == null)
                    return Results.NotFound("Deck not found");


                ICollection<CardDeckReadDTO> cardDTO = mapper.Map<ICollection<CardDeckReadDTO>>(deck.CardDecks); ;

                return Results.Ok(cardDTO);
            });

            // add a card to a deck by deck ID and card ID
            group.MapPost("/", (IDeckRepository repository, long deckId, [FromBody] AddCardDTO card) => 
            {
                var deck = repository.getDeck(deckId);
                if (deck == null)
                    return Results.NotFound("Deck not found");

                repository.AddCardToDeck(deckId, card.CardId);

                return Results.Ok("card added to deck successfully");
            });


            // Delete a card from a deck by deck ID and card ID
            group.MapDelete("/{cardId}", (IDeckRepository repository, long deckId, long cardId) =>
            {
                var deck = repository.getDeck(deckId);
                if (deck == null)
                    return Results.NotFound("Deck not found");

                bool cardRemoved = repository.RemoveCardFromDeck(deckId, cardId);
                if (!cardRemoved)
                    return Results.NotFound("Card not found in deck");

                return Results.Ok("Card removed from deck successfully");
            });
            

            // Update a card within a deck by deck ID and card ID
            group.MapPatch("/{cardId}", (IDeckRepository repository, long deckId, long cardId, [FromBody] int amount) =>
            {
                var deck = repository.getDeck(deckId);
                if (deck == null)
                    return Results.NotFound("Deck not found");

                bool cardUpdated = repository.UpdateCardAmountInDeck(deckId, cardId, amount);
                if (!cardUpdated)
                    return Results.NotFound("Card not found in deck");

                return Results.Ok("Card updated successfully");
            });

            return group;
        }
    }
}
