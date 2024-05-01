using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class CardExtensions
    {


        public static IQueryable<Card> ToFilteredList(this IQueryable<Card> cards, string artistName, string setName, string rarityName, string cardText, string cardName)
        {
            return cards.Where(card => card.Artist.FullName.StartsWith(artistName)
                                && card.Set.Name.StartsWith(setName)
                                && card.Rarity.Name.StartsWith(rarityName)
                                && card.Text.Contains(cardText)
                                && card.Name.StartsWith(cardName));
        }
    }
}
