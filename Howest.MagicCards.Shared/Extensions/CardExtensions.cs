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
        public static IQueryable<Card> Filter(this IQueryable<Card> cards, CardFilter cardFilter)
        {
            return cards.Where(card => card.Artist.FullName.StartsWith(cardFilter.ArtistName)
                                && card.SetCode.Contains(cardFilter.SetCode)
                                && card.RarityCode == cardFilter.RarityCode
                                && card.Text.Contains(cardFilter.CardText)
                                && card.Name.StartsWith(cardFilter.CardName));
        }
    }
}
