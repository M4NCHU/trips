using backend.Models;
using System;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ICategoryRepository : IRepository<CategoryModel>
    {
        Task<bool> CategoryExistsAsync(Guid id);
    }
}
