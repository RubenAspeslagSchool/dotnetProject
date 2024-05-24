using Howest.MagicCards.Web.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Register HttpClientFactory and configure named clients
builder.Services.AddHttpClient("CardsAPI", client =>
{
    // Replace this with your actual Cards API base address
    client.BaseAddress = new Uri("https://localhost:7195/api/V1.5/");
});

builder.Services.AddHttpClient("DecksAPI", client =>
{
    // Replace this with your actual Decks API base address
    client.BaseAddress = new Uri("https://localhost:7079/");
});

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
