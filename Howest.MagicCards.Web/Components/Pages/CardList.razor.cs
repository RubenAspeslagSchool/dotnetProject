using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.ViewModels;
using Howest.MagicCards.WebAPI.Wrappers;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class CardList : ComponentBase
    {
        [Parameter] public CardFilterViewModel CardFilterViewModel { get; set; }
        [Parameter] public EventCallback AddCardToDeck { get; set; }
        [Parameter] public EventCallback<CardReadDTO> AddCardToDeckCallback { get; set; } 
        public IEnumerable<CardReadDTO> CardsList { get; set; }
        private IEnumerable<CardReadDTO>? _cards = null;
        public EventCallback<CardReadDTO> OnCardClick { get; set; }
        private List<CardDetailDTO> _cardDetailsFromApi { get; set; } = new List<CardDetailDTO>();
        private Dictionary<long, bool> cardDetailVisibility = new Dictionary<long, bool>();
        private Dictionary<long, bool> cardDetailFromApiVisibility = new Dictionary<long, bool>();
        private HttpClient _cardsHttpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }


        public CardList() 
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        protected override async Task OnInitializedAsync()
        {
            _cardsHttpClient = HttpClientFactory.CreateClient("CardsAPI");
            await ShowAllCards();

        }

        public async Task ShowAllCards()
        {
            // Set default values if they are null
            CardFilterViewModel.MaxPageSize ??= 30;
            CardFilterViewModel.PageNumber ??= 1;
            CardFilterViewModel.PageSize ??= 30;

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
            CardFilterViewModel.MaxPageSize ??= 30;
            CardFilterViewModel.PageNumber ??= 1;
            CardFilterViewModel.PageSize ??= 30;

            var properties = CardFilterViewModel.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(CardFilterViewModel);
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

        // Method to hide card details
        public void HideCardDetails(long cardId)
        {
            if (cardDetailVisibility.ContainsKey(cardId))
            {
                cardDetailVisibility[cardId] = false;
            }
            StateHasChanged();
        }

    }
}
