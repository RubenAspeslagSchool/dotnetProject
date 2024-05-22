using Microsoft.AspNetCore.Components;
using Howest.MagicCards.Shared.DTO;
using System.Text.Json;
using Howest.MagicCards.WebAPI.Wrappers;
using Howest.MagicCards.Shared.ViewModels;
using AutoMapper;
using System.Text;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Howest.MagicCards.Web.Pages
{
    public partial class Cards : ComponentBase
    {
        private IEnumerable<CardReadDTO>? _cards = null;
        private IEnumerable<RarirtyReadDTO>? _rarties = null;
        private IList<DeckCardViewModel> _cardsInDeck { get; set; } = new List<DeckCardViewModel>();
        private IEnumerable<DeckReadDTO>? _olderDecks { get; set; }

        private CardFilterViewModel _cardFilterViewModel;
        private DeckViewModel _deckViewModel;

        private readonly JsonSerializerOptions _jsonOptions;
        private HttpClient _cardsHttpClient;
        private HttpClient _decksHttpClient;

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        [Inject]
        public IMapper mapper { get; set; }

        [Inject]
        public ProtectedLocalStorage storage { get; set; }

        public Cards()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        protected override async Task OnInitializedAsync()
        {
            _cardFilterViewModel = new CardFilterViewModel();
            _deckViewModel = new DeckViewModel();
            _cardsHttpClient = HttpClientFactory.CreateClient("CardsAPI");
            _decksHttpClient = HttpClientFactory.CreateClient("DecksAPI");

            await ShowAllCards();
            _rarties = await GetAllRarities();
            _olderDecks = await GetAllDecks();
        }

        private async Task<IEnumerable<DeckReadDTO>?> GetAllDecks()
        {
            HttpResponseMessage response = await _cardsHttpClient.GetAsync($"decks");

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<DeckReadDTO>? result =
                    JsonSerializer.Deserialize<IEnumerable<DeckReadDTO>>(apiResponse, _jsonOptions);
                return result;
            }
            else
            {
                return new List<DeckReadDTO>();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ProtectedBrowserStorageResult<IList<DeckCardViewModel>> storageResult = await storage.GetAsync<IList<DeckCardViewModel>>("ViewedDeck");
                _cardsInDeck = storageResult.Success ? storageResult.Value : new List<DeckCardViewModel>();
            }
        }

        private async Task ShowAllCards()
        {
            HttpResponseMessage response = await _cardsHttpClient.GetAsync("cards?" + GetQueryString());

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                PagedResponse<IEnumerable<CardReadDTO>>? result =
                        JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardReadDTO>>>(apiResponse, _jsonOptions);
                _cards = result?.Data;
            }
            else
            {
                _cards = new List<CardReadDTO>();
            }
        }

        private async Task<IEnumerable<RarirtyReadDTO>> GetAllRarities()
        {
            HttpResponseMessage response = await _cardsHttpClient.GetAsync($"rarities");

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<RarirtyReadDTO>? result =
                    JsonSerializer.Deserialize<IEnumerable<RarirtyReadDTO>>(apiResponse, _jsonOptions);
                return result;
            }
            else
            {
                return new List<RarirtyReadDTO>();
            }
        }

        private string GetQueryString()
        {
            string queryString = string.Empty;

            _cardFilterViewModel.GetType().GetProperties().ToList().ForEach(prop =>
            {
                string? value = prop.GetValue(_cardFilterViewModel) is string propValue ? propValue : null;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    queryString += $"&{prop.Name}={value}";
                }
            });

            return queryString;
        }

        private async void AddCardToDeck(CardReadDTO card)
        {
            DeckCardViewModel? cardViewModel = _cardsInDeck.FirstOrDefault(c => c.CardId == int.Parse(card.Id));

            if (cardViewModel is null)
            {
                _cardsInDeck.Add(new DeckCardViewModel { Amount = 1, CardId = int.Parse(card.Id), CardName = card.Name });
            }
            else
            {
                cardViewModel.Amount++;
            }

            await storage.SetAsync("ViewedDeck", _cardsInDeck);
        }


        private async void RemoveCard(DeckCardViewModel card)
        {
            DeckCardViewModel? cardViewModel = _cardsInDeck.FirstOrDefault(c => c.CardId == card.CardId);

            if (cardViewModel?.Amount > 1)
            {
                cardViewModel.Amount--;
            }
            else
            {
                _cardsInDeck.Remove(cardViewModel);
            }

            await storage.SetAsync("ViewedDeck", _cardsInDeck);
        }

        private async Task AddDeck()
        {
            _deckViewModel.DeckCards = _cardsInDeck;
            DeckCreateDTO deckWriteDTO = mapper.Map<DeckCreateDTO>(_deckViewModel);

            HttpContent content =
            new StringContent(JsonSerializer.Serialize(deckWriteDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _decksHttpClient.PostAsync("decks", content);

            if (response.IsSuccessStatusCode)
            {
                _olderDecks = await GetAllDecks();
            }

        }

        private async Task RemoveDeck(int id)
        {
            HttpResponseMessage response = await _decksHttpClient.DeleteAsync($"decks/{id}");

            if (response.IsSuccessStatusCode)
            {
                _olderDecks = await GetAllDecks();
            }
        }
    }
}