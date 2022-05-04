using RookieOnlineAssetManagement.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryModel>> GetAll();

        public Task<int> Create(CreateCategoryRequest request);

        public Task<CategoryModel> GetById(int id);
    }
}