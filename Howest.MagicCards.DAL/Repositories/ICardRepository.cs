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
        List<Card> GetAllCardsByArtistId(long ArtistId);
        public Task<List<Card>> GetCardsByPageAsync(int page, int pageSize);
    }
}
