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
                List<Deck> decks = repository.getDecks();
                List<DeckReadDTO> deckDTOs = mapper.Map<List<DeckReadDTO>>(decks);
                return Results.Ok(deckDTOs);
            });

            group.MapPost("/", (IDeckRepository repository, IMapper mapper, [FromBody] DeckCreateDTO createDeckDTO) =>
               repository.AddDeck(mapper.Map<Deck>(createDeckDTO))
             );

            group.MapPatch("/{id}", (IDeckRepository repository, IMapper mapper, int id, [FromBody] DeckCreateDTO updatedDeckDTO) =>
            {
                try
                {
                    repository.UbdateDeckName(id, updatedDeckDTO.DeckName);
                    return Results.Ok("Deck updated successfully");
                }
                catch (Exception)
                {
                    return Results.NotFound("Deck not found");
                }
                
            });

            group.MapDelete("/{id}", (IDeckRepository repository, int id) =>
            {
                try
                {
                    repository.RemoveDeck(id);
                    return Results.Ok("Deck deleted successfully");
                }
                catch (Exception)
                {
                    return Results.NotFound("Deck not found");
                }   
            });

            return group;
        }
    }
}
