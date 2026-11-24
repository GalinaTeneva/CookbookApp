using Cookbook.Data.Models;
using Cookbook.Data.Repository.Contracts;

namespace Cookbook.Data.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(CookbookDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
