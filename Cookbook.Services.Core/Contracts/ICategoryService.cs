using Cookbook.Services.Data.Genre;

namespace Cookbook.Services.Core.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetAllCategoriesResponseData>> GetAllCategoriesAsync();

        Task<bool> CategoryExistsAsync(int categoryId);
    }
}
