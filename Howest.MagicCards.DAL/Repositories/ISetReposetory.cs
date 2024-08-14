using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ISetReposetory
    {
        Task<List<Set>> GetAllSetsAsync();
    }
}