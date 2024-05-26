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

        private Dictionary<long, bool> cardDetailVisibility = new Dictionary<long, bool>();
        private Dictionary<long, bool> cardDetailFromApiVisibility = new Dictionary<long, bool>();

        public EventCallback<CardReadDTO> OnCardClick { get; set; }
        private List<CardDetailDTO> _cardDetailsFromApi { get; set; }
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

            _cardDetailsFromApi = new List<CardDetailDTO>();

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

        private async void AddCardToDeck(CardReadDTO card)
        {
            DeckCardViewModel? cardViewModel = _cardsInDeck.FirstOrDefault(c => c.CardId == long.Parse( card.Id));

            if (cardViewModel is null)
            {

                _cardsInDeck.Add(new DeckCardViewModel { Amount = 1, CardId = long.Parse( card.Id), CardName= card.Name });
            }
            else
            {
                cardViewModel.Amount++;
            }

            await storage.SetAsync("ViewedDeck", _cardsInDeck);
        }

        private async void AddCardIdToDeck(long? cardId)
        {
            DeckCardViewModel? cardViewModel = _cardsInDeck.FirstOrDefault(c => c.CardId == cardId);
           cardViewModel.Amount++;
            await storage.SetAsync("ViewedDeck", _cardsInDeck);
        }

        // Method to show card details
        public void ShowCardDetails(long cardId)
        {
            Console.WriteLine("showing cardDetails ...");
            if (!cardDetailVisibility.ContainsKey(cardId))
            {
                cardDetailVisibility[cardId] = false;
            }

            cardDetailVisibility[cardId] = true;
            StateHasChanged();
        }

        // Method to hide card details
        public void HideCardDetails(long cardId)
        {
            if (cardDetailVisibility.ContainsKey(cardId))
            {
                cardDetailVisibility[cardId] = false;
            }
            StateHasChanged();
        }

        // Method to show additional card details from API
        public async void ShowCardDetailsFromApi(long cardId)
        {
            if (!cardDetailFromApiVisibility.ContainsKey(cardId))
            {
                cardDetailFromApiVisibility[cardId] = false;
            }

            cardDetailFromApiVisibility[cardId] = true;

            // Fetch the extended details from API
            HttpResponseMessage response = await _cardsHttpClient.GetAsync($"cards/{cardId}");
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                // Deserialize and store the extended details as needed
                // Assuming CardDetailDTO is the extended detail DTO
                CardDetailDTO? cardDetail = JsonSerializer.Deserialize<CardDetailDTO>(apiResponse, _jsonOptions);
                _cardDetailsFromApi.Add(cardDetail);
            }
            else
            {
                Console.WriteLine($"Failed to fetch card details for card ID {cardId}. Status Code: {response.StatusCode}");
            }
            StateHasChanged();
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

        private async void RemoveCard(DeckCardViewModel card)
        {
            Console.WriteLine("removing card" + card);
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
            StateHasChanged();
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
            Console.WriteLine("removing deck" +  id + "...");
            HttpResponseMessage response = await _decksHttpClient.DeleteAsync($"decks/{id}");
            Console.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                _allDecks = await GetAllDecks();
            }
            StateHasChanged();
        }
    }
}
