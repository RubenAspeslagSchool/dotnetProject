using AutoMapper;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.ViewModels;
using Howest.MagicCards.WebAPI.Wrappers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;
using System.Text;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Forms;
using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class Home : ComponentBase
    {
        public IEnumerable<CardReadDTO> CardsList { get; set; }

      
        public EventCallback<CardReadDTO> OnCardClick { get; set; }
        private IEnumerable<CardReadDTO>? _cards = null;
        private IEnumerable<RarirtyReadDTO>? _rarties = null;
        private IList<DeckCardViewModel> _cardsInDeck { get; set; } = new List<DeckCardViewModel>();
        private IEnumerable<DeckReadDTO>? _allDecks { get; set; }

        private CardFilterViewModel _cardFilterViewModel;
        private DeckViewModel _deckViewModel;

        private readonly JsonSerializerOptions _jsonOptions;
        private HttpClient _cardsHttpClient;
        private HttpClient _decksHttpClient;
        private Deck _currentDeck { get; set; } = new Deck();

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        [Inject]
        public IMapper mapper { get; set; }

        [Inject]
        public ProtectedLocalStorage storage { get; set; }

        public Home()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        protected override async Task OnInitializedAsync()
        {
            _cardFilterViewModel = new CardFilterViewModel();
            _deckViewModel = new DeckViewModel
            {
                DeckName = "My Deck"
            };
            _cardsHttpClient = HttpClientFactory.CreateClient("CardsAPI");
            _decksHttpClient = HttpClientFactory.CreateClient("DecksAPI");

            await ShowAllCards();
            _rarties = await GetAllRarities();
            _allDecks = await GetAllDecks();
            _currentDeck = mapper.Map<Deck>(_allDecks?.OrderByDescending(deck => deck.Id).FirstOrDefault()) ;

        }

        private async Task<IEnumerable<DeckReadDTO>?> GetAllDecks()
        {
            HttpResponseMessage response = await _decksHttpClient.GetAsync($"decks");

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
            // Set default values if they are null
            _cardFilterViewModel.MaxPageSize ??= 150;
            _cardFilterViewModel.PageNumber ??= 1;
            _cardFilterViewModel.PageSize ??= 150;

            string queryString = GetQueryString();
            Console.WriteLine("Query String: " + queryString); // Log the query string

            HttpResponseMessage response = await _cardsHttpClient.GetAsync("cards?" + queryString);

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Response: " + apiResponse); // Add logging to see the response in the console

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                PagedResponse<IEnumerable<CardReadDTO>> result =
                    JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardReadDTO>>>(apiResponse, options);

                _cards = result?.Data ?? new List<CardReadDTO>();
                Console.WriteLine("Cards Count: " + _cards.Count()); // Log the count of cards received
            }
            else
            {
                Console.WriteLine("Failed to fetch cards. Status Code: " + response.StatusCode);
                _cards = new List<CardReadDTO>();
            }
        }

        private string GetQueryString()
        {
            StringBuilder queryStringBuilder = new StringBuilder();

            // TODO: Get default values from the config file
            _cardFilterViewModel.MaxPageSize ??= 150;
            _cardFilterViewModel.PageNumber ??= 1;
            _cardFilterViewModel.PageSize ??= 150;

            var properties = _cardFilterViewModel.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(_cardFilterViewModel);
                if (value != null)
                {
                    string stringValue = value.ToString();
                    if (!string.IsNullOrWhiteSpace(stringValue))
                    {
                        queryStringBuilder.Append($"&{prop.Name}={stringValue}");
                    }
                }
            }

            string queryString = queryStringBuilder.ToString();
            Console.WriteLine("Query String: " + queryString); // Log the query string

            return queryString.Length > 0 ? queryString.Substring(1) : queryString; // Remove leading '&' if exists
        }

        private async Task<IEnumerable<RarirtyReadDTO>> GetAllRarities()
        {
            HttpResponseMessage response = await _cardsHttpClient.GetAsync($"Rarirty");

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<RarirtyReadDTO>? result =
                    JsonSerializer.Deserialize<IEnumerable<RarirtyReadDTO>>(apiResponse, _jsonOptions);
                Console.WriteLine(result);
                return result;
            }
            else
            {
                Console.WriteLine(response);
                return new List<RarirtyReadDTO>();
            }
        }

        private async void AddCardToDeck(long cardId)
        {
            DeckCardViewModel? cardViewModel = _cardsInDeck.FirstOrDefault(c => c.CardId == cardId);

            if (cardViewModel is null)
            {
                _cardsInDeck.Add(new DeckCardViewModel { Amount = 1, CardId = cardId });
            }
            else
            {
                cardViewModel.Amount++;
            }

            await storage.SetAsync("ViewedDeck", _cardsInDeck);
        }

        public void ShowCardDetails(long cardId) 
        {
            
        }

        public void HideCardDetails(long cardId)
        {

        }

        public void ShowCardDetailsFromApi(long cardId) 
        {
            
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
            }
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
            DeckCreateDTO deckWriteDTO = mapper.Map<DeckCreateDTO>(_deckViewModel);

            HttpContent content =
            new StringContent(JsonSerializer.Serialize(deckWriteDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _decksHttpClient.PostAsync("decks", content);

            if (response.IsSuccessStatusCode)
            {
                _allDecks = await GetAllDecks();
            }

        }

        private async Task RemoveDeck(long id)
        {
            HttpResponseMessage response = await _decksHttpClient.DeleteAsync($"decks/{id}");

            if (response.IsSuccessStatusCode)
            {
                _allDecks = await GetAllDecks();
            }
        }
    }
}
