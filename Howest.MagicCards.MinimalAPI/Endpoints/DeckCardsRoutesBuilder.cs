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
            group.MapGet("/", (IMapper mapper, IDeckRepository repository, long deckId) =>
            {
                try
                {
                    var deck = repository.getDeck(deckId);
                    ICollection<CardDeckReadDTO> cardDTO = mapper.Map<ICollection<CardDeckReadDTO>>(deck.CardDecks);
                    return Results.Ok(cardDTO);
                } catch (Exception ex)
                {
                    return Results.NotFound("Deck not found");
                }               
            });

            // add a card to a deck by deck ID and card ID
            group.MapPost("/", (IDeckRepository repository, long deckId, [FromBody] CardAddDTO card) => 
            {
                try
                {
                    repository.AddCardToDeck(deckId, card.CardId);
                    return Results.Ok("card added to deck successfully");
                }
                catch (Exception)
                {

                    return Results.NotFound("Deck not found");
                }
            });

            // Delete a card from a deck by deck ID and card ID
            group.MapDelete("/{cardId}", (IDeckRepository repository, long deckId, long cardId) =>
            {
                try
                {
                    repository.RemoveCardFromDeck(deckId, cardId);
                    return Results.Ok("Card removed from deck successfully");
                }
                catch (Exception)
                {
                   return Results.NotFound("Deck not found");
                }
            });
            

            // Update a card within a deck by deck ID and card ID
            group.MapPatch("/{cardId}", (IDeckRepository repository, long deckId, long cardId, [FromBody] int amount) =>
            {
                try
                {
                    repository.UpdateCardAmountInDeck(deckId, cardId, amount);
                    return Results.Ok("Card updated successfully");
                } catch (Exception)
                {
                    return Results.NotFound("Deck not found in deck");
                }
            });
            return group;
        }
    }
}
