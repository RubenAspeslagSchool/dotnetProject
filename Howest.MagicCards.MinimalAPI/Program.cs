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

app.MapGet("/decks", (IDeckReposetory repository) =>
        repository.getDecks()
    );

app.MapPost("/decks", (IDeckReposetory repository, IMapper mapper, [FromBody] DeckCreateDTO createDeckDTO) =>
   repository.AddDeck(mapper.Map<Deck>(createDeckDTO))
    ) ;

app.Run();

