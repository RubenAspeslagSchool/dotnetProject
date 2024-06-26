﻿using AutoMapper;
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
                List<Deck> decks = repository.Decks;
                List<DeckReadDTO> deckDTOs = mapper.Map<List<DeckReadDTO>>(decks);
                return Results.Ok(deckDTOs);
            });

            group.MapPost("/", (IDeckRepository repository, IMapper mapper, [FromBody] DeckCreateDTO createDeckDTO) =>
               repository.CreateDeck(createDeckDTO.DeckName)
             );

            group.MapPatch("/{id}", (IDeckRepository repository, IMapper mapper, int id, [FromBody] DeckCreateDTO updatedDeckDTO) =>
            {
                try
                {
                    repository.UpdateDeckName(id, updatedDeckDTO.DeckName);
                    return Results.Ok("Deck updated successfully");
                }
                catch (ArgumentNullException ex)
                {
                    return Results.NotFound(ex.Message);
                }

            });

            group.MapDelete("/{id}", (IDeckRepository repository, int id) =>
            {
                try
                {
                    repository.RemoveDeck(id);
                    return Results.Ok("Deck deleted successfully");
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
