using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class CardExtensions
    {
        // TODO: use separate parameters for filter to avoid passing pagination pparameters to the method
        public static IQueryable<Card> Filter(this IQueryable<Card> cards, CardFilter cardFilter)
        {
            // make sure filter is skipped when value is null
            return cards.Where(card => card.Artist.FullName.StartsWith(cardFilter.ArtistName)
                                && card.SetCode.Contains(cardFilter.SetCode)
                                && card.RarityCode == cardFilter.RarityCode
                                && card.Text.Contains(cardFilter.CardText)
                                && card.Name.StartsWith(cardFilter.CardName));
        }
    }
}
