using Cookbook.Data.Models;

namespace Cookbook.Data.Repository.Contracts
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        Task<List<Ingredient>> FindByNormalizedNamesAsync(
    ICollection<string> normalizedNames);
    }
}
