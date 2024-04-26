using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Howest.MagicCards.Shared.DTO;
using AutoMapper;
using Howest.MagicCards.DAL.Repositories;
using Type = System.Type;
using Howest.MagicCards.MinimalAPI.Endpoints;

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

app.MapGroup("/decks")
    .MapDeckApi()
    .WithTags("Deck");

app.MapGroup("/decks/{deckId}/cards")
    .MapCardDeckApi()
    .WithTags("DeckCards (cards of a deck)");

app.Run();

