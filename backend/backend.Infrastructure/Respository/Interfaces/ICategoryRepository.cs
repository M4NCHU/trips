using backend.Domain.DTOs;
using backend.Models;
using System;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ICategoryRepository : IRepository<CategoryModel>
    {
        Task<PagedResult<CategoryModel>> GetPagedCategories(int page, int pageSize);
        Task<bool> CategoryExistsAsync(Guid id);
        Task<bool> CategoryExistsAsync(string name);
        public IQueryable<CategoryModel> GetAllCategoriesAsQueryable();
    }
}
