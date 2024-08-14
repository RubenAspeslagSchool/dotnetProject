namespace Howest.MagicCards.DAL.Repositories
{
    public interface ITypesReposetory
    {
        Task<List<Models.Type>> GetAllTypesAsync();
    }
}