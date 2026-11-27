using Cookbook.Data.Models;
using Cookbook.Data.Repository.Contracts;

namespace Cookbook.Data.Repository
{
    public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(CookbookDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
