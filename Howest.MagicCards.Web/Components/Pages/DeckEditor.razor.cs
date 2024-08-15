using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class DeckEditor : ComponentBase
    {
        private IEnumerable<DeckReadDTO>? _allDecks { get; set; }
        private Deck _currentDeck { get; set; } = new Deck();
        private readonly JsonSerializerOptions _jsonOptions;
        private HttpClient _decksHttpClient;
        private DeckViewModel _deckViewModel;
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject]
        public ProtectedLocalStorage storage { get; set; }
        private IList<DeckCardViewModel> _cardsInDeck { get; set; } = new List<DeckCardViewModel>();



        [Inject]
        public IMapper mapper { get; set; }

        public DeckEditor()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        protected override async Task OnInitializedAsync()
        {
            _deckViewModel = new DeckViewModel
            {
                DeckName = "My Deck"
            };
            _decksHttpClient = HttpClientFactory.CreateClient("DecksAPI");
            _allDecks = await GetAllDecks();
            _currentDeck = mapper.Map<Deck>(_allDecks?.OrderByDescending(deck => deck.Id).FirstOrDefault());

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ProtectedBrowserStorageResult<IList<DeckCardViewModel>> storageResult = await storage.GetAsync<IList<DeckCardViewModel>>("ViewedDeck");
                _cardsInDeck = storageResult.Success ? storageResult.Value : new List<DeckCardViewModel>();
            }
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
        public async void AddCardToDeck(CardReadDTO card)
        {
            DeckCardViewModel? cardViewModel = _cardsInDeck.FirstOrDefault(c => c.CardId == long.Parse(card.Id));

            if (cardViewModel is null)
            {

                _cardsInDeck.Add(new DeckCardViewModel { Amount = 1, CardId = long.Parse(card.Id), CardName = card.Name });
            }
            else
            {
                cardViewModel.Amount++;
            }

            await storage.SetAsync("ViewedDeck", _cardsInDeck);
        }

        public async void AddCardIdToDeck(long? cardId)
        {
            DeckCardViewModel? cardViewModel = _cardsInDeck.FirstOrDefault(c => c.CardId == cardId);
            cardViewModel.Amount++;
            await storage.SetAsync("ViewedDeck", _cardsInDeck);
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
            Console.WriteLine("removing deck" + id + "...");
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

