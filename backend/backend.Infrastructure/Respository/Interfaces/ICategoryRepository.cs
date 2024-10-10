using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryModel>> GetCategoriesAsync();
        Task<CategoryModel> GetCategoryByIdAsync(Guid id);
        Task AddCategoryAsync(CategoryModel category);
        Task UpdateCategoryAsync(CategoryModel category);
        Task DeleteCategoryAsync(Guid id);
        Task<bool> CategoryExistsAsync(Guid id);
    }
}
