using Cookbook.Data.Models;
using Cookbook.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data.Repository
{
    public class IngredientRepository : BaseRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(CookbookDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<List<Ingredient>> FindByNormalizedNamesAsync(ICollection<string> normalizedNames)
        {
            if(normalizedNames.Count == 0)
                return Task.FromResult(new List<Ingredient>());

            string[] names = normalizedNames.ToArray();

            return DbSet.Where(i => names.Contains(i.Name.ToLower())).ToListAsync();
        }
    }
}
