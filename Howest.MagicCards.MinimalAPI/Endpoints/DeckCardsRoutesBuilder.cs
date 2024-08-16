using AutoMapper;
using Howest.MagicCards.DAL.Exceptions;
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
                    Deck deck = repository.GetDeck(deckId);
                    ICollection<CardDeckReadDTO> cardDTO = mapper.Map<ICollection<CardDeckReadDTO>>(deck.CardDecks);
                    return Results.Ok(cardDTO);
                } catch (ArgumentNullException ex)
                {
                    return Results.NotFound(ex.Message);
                }               
            });

            // add a card to a deck by deck ID and card ID
            group.MapPost("/", (IDeckRepository repository, long deckId, [FromBody] CardAddDTO card) => 
            {
                try
                {
                    repository.AddCardToDeck(deckId, card.CardId);
                    return Results.Ok($"card {card.CardId} added to deck {deckId} successfully");
                }
                catch (ArgumentNullException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (ToManyCardsInDeckExeption ex )
                {
                    return Results.Conflict(ex.Message); // Use 409 Conflict for cases where too many cards are in the deck
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
                catch (ArgumentNullException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            });
            

            // Update a card within a deck by deck ID and card ID  
            
            group.MapPatch("/{cardId}", (IDeckRepository repository, long deckId, long cardId, [FromBody] CardUbdateAmountDTO amountDTO) =>
            {
                try
                {
                    repository.UpdateCardAmountInDeck(deckId, cardId, amountDTO.Amount);
                    return Results.Ok("Card updated successfully");
                }
                catch (ArgumentNullException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            });
            return group;
        }
    }
}
