using Cookbook.Data.Models;

namespace Cookbook.Data.Repository.Contracts
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        Task<Recipe?> FindByIdAsync(int id);
    }
}
