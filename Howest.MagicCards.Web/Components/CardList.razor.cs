using Howest.MagicCards.Shared.DTO;


// using Howest.MagicCards.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
namespace Howest.MagicCards.Web.Pages
{
    public partial class CardListBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<CardReadDTO> Cards { get; set; }

        [Parameter]
        public EventCallback<CardReadDTO> OnCardClick { get; set; }

    }
}
