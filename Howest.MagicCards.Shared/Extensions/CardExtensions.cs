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
            string rarityCode,
            string cardType)
        {
            IQueryable<Card> filteredCards = cards;

            if (!string.IsNullOrEmpty(artistName))
            {
                filteredCards = filteredCards.Where(card => card.Artist.FullName.StartsWith(artistName));
            }

            if (!string.IsNullOrEmpty(setCode))
            {
                filteredCards = filteredCards.Where(card => card.SetCode == setCode);
            }

            if (!string.IsNullOrEmpty(rarityCode))
            {
                filteredCards = filteredCards.Where(card => card.RarityCode == rarityCode);
            }

            if (!string.IsNullOrEmpty(cardType))
            {
                filteredCards = filteredCards.Where(card => card.Type == cardType);
            }

            if (!string.IsNullOrEmpty(cardText))
            {
                filteredCards = filteredCards.Where(card => card.Text.StartsWith(cardText));
            }

            if (!string.IsNullOrEmpty(cardName))
            {
                filteredCards = filteredCards.Where(card => card.Name.StartsWith(cardName));
            }

            // Apply default ordering
            return filteredCards.OrderBy(card => card.Id);
        }


        public static IQueryable<Card> ApplySorting(this IQueryable<Card> query, string orderBy)
        {
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                return orderBy.ToLower() switch
                {
                    "ascending" => query.OrderBy(card => card.Name),
                    "descending" => query.OrderByDescending(card => card.Name),
                    _ => query,
                };
            } return query.OrderBy(card => card.Id);
        }

       
    }
}
