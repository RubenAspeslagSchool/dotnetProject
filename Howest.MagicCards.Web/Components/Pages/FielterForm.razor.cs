using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class FielterForm : ComponentBase
    {
        [Parameter] public CardFilterViewModel CardFilterViewModel { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }
        private IEnumerable<RarityReadDTO>? _rarties = null;
        private IEnumerable<SetReadDTO>? _sets = null;
        private IEnumerable<ArtistReadDTO>? _artists = null;
        private IEnumerable<TypeReadDTO>? _types = null;
        private readonly JsonSerializerOptions _jsonOptions;

       

        private HttpClient _cardsHttpClient;
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        public FielterForm()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        protected override async Task OnInitializedAsync()
        {
            _cardsHttpClient = HttpClientFactory.CreateClient("CardsAPI");
            await getFilterFormDataFromApi();

        }

       
        private async Task getFilterFormDataFromApi()
        {
            _rarties = await GetFielterDataFromApiEndpoint<RarityReadDTO>("Raritys");
            _sets = await GetFielterDataFromApiEndpoint<SetReadDTO>("Sets");
            _artists = await GetFielterDataFromApiEndpoint<ArtistReadDTO>("Artists");
            _types = await GetFielterDataFromApiEndpoint<TypeReadDTO>("Types");
        }


        private async Task<IEnumerable<T>> GetFielterDataFromApiEndpoint<T>(string apiEndpoint)
        {
            HttpResponseMessage response = await _cardsHttpClient.GetAsync(apiEndpoint);

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<T>? result = JsonSerializer.Deserialize<IEnumerable<T>>(apiResponse, _jsonOptions);
                Console.WriteLine(result);
                return result ?? new List<T>();
            }
            else
            {
                Console.WriteLine(response);
                return new List<T>();
            }
        }

    }
}
