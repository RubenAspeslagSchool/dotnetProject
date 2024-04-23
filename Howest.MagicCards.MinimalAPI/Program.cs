using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Howest.MagicCards.Shared.DTO;
using AutoMapper;
using Howest.MagicCards.DAL.Repositories;
using Type = System.Type;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDeckReposetory, JsonDeckReposetory>();
builder.Services.AddAutoMapper(new Type[] { 
    typeof(Howest.MagicCards.Shared.Maping.DecksProfile) 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello world");

app.MapGet("/decks", (IDeckReposetory repository, IMapper mapper) =>
{
    var decks = repository.getDecks();
    var deckDTOs = mapper.Map<List<DeckReadDTO>>(decks);
    return Results.Ok(deckDTOs);
});

app.MapPost("/decks", (IDeckReposetory repository, IMapper mapper, [FromBody] DeckCreateDTO createDeckDTO) =>
   repository.AddDeck(mapper.Map<Deck>(createDeckDTO))
    ) ;




app.MapPut("/decks/{id}", (IDeckReposetory repository, IMapper mapper, int id, [FromBody] DeckCreateDTO updatedDeckDTO) =>
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
app.MapDelete("/decks/{id}", (IDeckReposetory repository, int id) =>
{
    var deck = repository.getDeck(id);
    if (deck == null)
        return Results.NotFound("Deck not found");

    repository.RemoveDeck(id);

    return Results.Ok("Deck deleted successfully");
});


# region cards

// Update a card within a deck by deck ID and card ID
app.MapPut("/decks/{deckId}/cards/{cardId}", (IDeckReposetory repository, int deckId, long cardId, [FromBody] int amount) =>
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
app.MapDelete("/decks/{deckId}/cards/{cardId}", (IDeckReposetory repository, int deckId, long cardId) =>
{
    var deck = repository.getDeck(deckId);
    if (deck == null)
        return Results.NotFound("Deck not found");

    bool cardRemoved = repository.RemoveCardFromDeck(deckId, cardId);
    if (!cardRemoved)
        return Results.NotFound("Card not found in deck");

    return Results.Ok("Card removed from deck successfully");
});

#endregion 

app.Run();

