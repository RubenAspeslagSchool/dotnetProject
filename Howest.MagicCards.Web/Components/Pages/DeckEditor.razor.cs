﻿using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class DeckEditor : ComponentBase
    {
        private IEnumerable<DeckReadDTO>? _allDecks { get; set; }
        private readonly JsonSerializerOptions _jsonOptions;
        private HttpClient _decksHttpClient;
        private DeckViewModel _deckViewModel;
        private long _currentDeckId;
        private Dictionary<long, string> _cardNames = new();

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        [Inject]
        public IMapper mapper { get; set; }

        public DeckEditor()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public void SetAsCurrentDeck(long currentDeckId)
        {
            _currentDeckId = currentDeckId;
            StateHasChanged(); 
        }

        protected override async Task OnInitializedAsync()
        {
            _deckViewModel = new DeckViewModel
            {
                DeckName = "My Deck"
            };
            _decksHttpClient = HttpClientFactory.CreateClient("DecksAPI");
            await RefreshDecks(); // Load initial data
            _currentDeckId = _allDecks?.OrderBy(deck => deck.Id).FirstOrDefault()?.Id ?? 0;
        }


        public async Task<string> GetCardNameById(long cardId)
        {
            try
            {
                HttpClient cardsHttpClient = HttpClientFactory.CreateClient("CardsAPI");
                HttpResponseMessage response = await cardsHttpClient.GetAsync($"cards/{cardId}");

                if (response.IsSuccessStatusCode)
                {
                    var cardDetail = await response.Content.ReadFromJsonAsync<CardDetailDTO>();
                    return cardDetail?.Name ?? "Card name not found";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return "Card not found";
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        private async Task<IEnumerable<DeckReadDTO>?> GetAllDecks()
        {
            HttpResponseMessage response = await _decksHttpClient.GetAsync("decks");

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<IEnumerable<DeckReadDTO>>(apiResponse, _jsonOptions);
            }
            else
            {
                return new List<DeckReadDTO>();
            }
        }

        public async Task AddCardToCurrentDeck(string cardId)
        {
            await AddCardToDeck(_currentDeckId, long.Parse(cardId));
        }

        public async Task AddCardToDeck(long deckId, long cardId)
        {
            try
            {
                HttpContent content = JsonContent.Create(new CardAddDTO { CardId = cardId });
                HttpResponseMessage response = await _decksHttpClient.PostAsync($"decks/{deckId}/cards", content);

                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(responseBody);
                    await RefreshDecks(); // Refresh decks to reflect changes
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    Console.WriteLine("Conflict: Too many cards in the deck.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Not Found: Deck or card not found.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private async void RemoveCardFromDeck(long deckId, long cardId)
        {
            try
            {
                var uri = $"decks/{deckId}/cards/{cardId}";
                HttpResponseMessage response = await _decksHttpClient.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.Content);
                    await RefreshDecks(); // Refresh decks to reflect changes
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Not Found: Deck or card not found.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private async Task RemoveDeck(long id)
        {
            Console.WriteLine("removing deck" + id + "...");
            HttpResponseMessage response = await _decksHttpClient.DeleteAsync($"decks/{id}");
            Console.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                await RefreshDecks(); // Refresh decks to reflect changes
            }
            StateHasChanged();
        }


        private async Task RefreshDecks()
        {
            _allDecks = await GetAllDecks();
            _cardNames.Clear(); // Clear existing card names to reload them
            if (_allDecks is not null)
            {
                foreach (var deck in _allDecks)
                {
                    foreach (var deckCard in deck.CardDecks)
                    {
                        if (!_cardNames.ContainsKey(deckCard.CardId))
                        {
                            var cardName = await GetCardNameById(deckCard.CardId);
                            _cardNames[deckCard.CardId] = cardName;
                        }
                    }
                }
            }
            StateHasChanged(); // Notify the UI to refresh
        }




        private async Task HandleAddDeckSubmit(EditContext editContext)
        {
            Console.WriteLine("Form is being submitted");
            Console.WriteLine("DeckViewModel before mapping: " + JsonSerializer.Serialize(_deckViewModel));
            DeckCreateDTO deckWriteDTO = mapper.Map<DeckCreateDTO>(_deckViewModel);
            Console.WriteLine("DeckCreateDTO after mapping: " + JsonSerializer.Serialize(deckWriteDTO));
            HttpContent content = new StringContent(JsonSerializer.Serialize(deckWriteDTO), Encoding.UTF8, "application/json");
            Console.WriteLine("add deck request body: " + JsonSerializer.Serialize(deckWriteDTO));
            HttpResponseMessage response = await _decksHttpClient.PostAsync("decks", content);
            if (response.IsSuccessStatusCode)
            {
                _allDecks = await GetAllDecks();
                StateHasChanged();
            }
        }
    }
}
