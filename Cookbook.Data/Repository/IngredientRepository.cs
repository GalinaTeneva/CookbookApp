using Cookbook.Data.Models;
using Cookbook.Data.Repository.Contracts;

namespace Cookbook.Data.Repository
{
    public class IngredientRepository : BaseRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(CookbookDbContext dbContext)
            : base(dbContext)
        {
            
        }
    }
}
