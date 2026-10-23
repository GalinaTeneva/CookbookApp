using Cookbook.Data;
using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Genre;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Services.Core
{
    public class CategoryService : ICategoryService
    {
        private readonly CookbookDbContext dbContext;

        public CategoryService(CookbookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await dbContext.Categories.AnyAsync(c => c.Id == categoryId);
        }

        public async Task<IEnumerable<GetAllCategoriesResponseData>> GetAllCategoriesAsync()
        {
            return await dbContext.Categories
                .Select(c => new GetAllCategoriesResponseData()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .OrderBy(c => c.Name)
                .ToArrayAsync();
        }
    }
}
