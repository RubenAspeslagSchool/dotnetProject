using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.DAL.Repositories;

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
            IQueryable<Card> filteredCards = cards.Where(card =>
                 (artistName == null || card.Artist.FullName.Contains(artistName))
              && (setCode == null || card.SetCode.Contains(setCode))
              && (rarityCode == null || card.RarityCode.Contains(rarityCode))
              && (cardText == null || card.Text.Contains(cardText))
              && (cardName == null || card.Name.StartsWith(cardName)));

            // Apply default ordering
            return filteredCards.OrderBy(card => card.Id);
        }

        public static IQueryable<Card> ApplySorting(this IQueryable<Card> query, string orderBy)
        {
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                return orderBy.ToLower() switch
                {
                    "name" => query.OrderBy(card => card.Name),
                    "artist" => query.OrderBy(card => card.Artist.FullName),
                    _ => query,
                };
            } return query.OrderBy(card => card.Id);
        }

        public async static  Task<List<Card>> ApplyPaging(this  IQueryable<Card> queryableCards, CardFilter cardFilter)
        {
            return  await queryableCards
            .Skip((cardFilter.PageNumber - 1) * cardFilter.PageSize)
            .Take(cardFilter.PageSize)
            .ToListAsync();
        }

        
    }
}
