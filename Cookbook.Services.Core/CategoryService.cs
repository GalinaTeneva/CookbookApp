using Cookbook.Data.Repository.Contracts;
using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Genre;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Services.Core
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await categoryRepository
                .AllAsNoTracking()
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<IEnumerable<GetAllCategoriesResponseData>> GetAllCategoriesAsync()
        {
            return await categoryRepository
                .AllAsNoTracking()
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
