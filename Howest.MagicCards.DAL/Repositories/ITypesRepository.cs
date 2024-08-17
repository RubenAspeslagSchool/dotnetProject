namespace Howest.MagicCards.DAL.Repositories
{
    public interface ITypesRepository
    {
        Task<List<Models.Type>> GetAllTypesAsync();
    }
}