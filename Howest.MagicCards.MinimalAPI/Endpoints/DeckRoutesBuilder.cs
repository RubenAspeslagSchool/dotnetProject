using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.MinimalAPI.Endpoints
{
    public static class DeckRoutesBuilder
    {
        public static RouteGroupBuilder MapDeckApi(this RouteGroupBuilder group)
        {
            group.MapGet("/", (IDeckRepository repository, IMapper mapper) =>
            {
                var decks = repository.getDecks();
                var deckDTOs = mapper.Map<List<DeckReadDTO>>(decks);
                return Results.Ok(deckDTOs);
            });

            group.MapPost("/", (IDeckRepository repository, IMapper mapper, [FromBody] DeckCreateDTO createDeckDTO) =>
               repository.AddDeck(mapper.Map<Deck>(createDeckDTO))
             );

            group.MapPatch("/{id}", (IDeckRepository repository, IMapper mapper, int id, [FromBody] DeckCreateDTO updatedDeckDTO) =>
            {
                var deck = repository.getDeck(id);
                if (deck == null)
                    return Results.NotFound("Deck not found");

                // Update deck properties with DTO data
                deck.DeckName = updatedDeckDTO.DeckName;

                // Save the updated deck
                repository.saveDeck(id, deck);

                return Results.Ok("Deck updated successfully");
            });

            // Delete a deck by ID
            group.MapDelete("/{id}", (IDeckRepository repository, int id) =>
            {
                var deck = repository.getDeck(id);
                if (deck == null)
                    return Results.NotFound("Deck not found");

                repository.RemoveDeck(id);

                return Results.Ok("Deck deleted successfully");
            });

            return group;
        }
    }
}
