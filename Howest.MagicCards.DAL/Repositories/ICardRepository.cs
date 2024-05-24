using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        Task<List<Card>> GetAllCardsAsync();
        IQueryable<Card> GetAllCards(); 
        Task<Card> GetCardByIdAsync(long id);
    }
}
