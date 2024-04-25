using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Howest.MagicCards.Shared.DTO;
using AutoMapper;
using Howest.MagicCards.DAL.Repositories;
using Type = System.Type;
using Howest.MagicCards.MinimalAPI.Endpoinds;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDeckRepository, JsonDeckRepository>();
builder.Services.AddAutoMapper(new Type[] { 
    typeof(Howest.MagicCards.Shared.Mapping.DecksProfile) 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGroup("/decks").MapDeckApi().WithTags("Deck");


# region cards

// Update a card within a deck by deck ID and card ID
app.MapPut("/decks/{deckId}/cards/{cardId}", (IDeckRepository repository, int deckId, long cardId, [FromBody] int amount) =>
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
app.MapDelete("/decks/{deckId}/cards/{cardId}", (IDeckRepository repository, int deckId, long cardId) =>
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

