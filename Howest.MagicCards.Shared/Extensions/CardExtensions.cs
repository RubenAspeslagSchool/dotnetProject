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
        public static IQueryable<Card> Filter(
            this IQueryable<Card> cards,
            string cardName,
            string cardText,
            string artistName,
            string setCode,
            string rarityCode
   )
        {
            var filteredCards = cards.Where(card =>
                 (artistName == null || card.Artist.FullName.Contains(artistName))
              && (setCode == null || card.SetCode.Contains(setCode))
              && (rarityCode == null || card.RarityCode.Contains(rarityCode))
              && (cardText == null || card.Text.Contains(cardText))
              && (cardName == null || card.Name.StartsWith(cardName)));

            // Apply default ordering
            return filteredCards.OrderBy(card => card.Id);
        }

    }
}
