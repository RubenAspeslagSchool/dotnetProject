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
        private CardList? cardListRef;
        private DeckEditor? deckEditorRef;
        private CardFilterViewModel _cardFilterViewModel;

        protected override async Task OnInitializedAsync()
        {
            _cardFilterViewModel = new CardFilterViewModel();

        }

        private async Task ShowAllCards()
        {
            if (cardListRef != null)
            {
                await cardListRef.ShowAllCards();
            }
        }

        private async Task AddCardToDeck(CardReadDTO card)
        {
            if (deckEditorRef != null)
            {
              await  deckEditorRef.AddCardToCurrentDeck(card.Id);
            }
        }
    }
}
