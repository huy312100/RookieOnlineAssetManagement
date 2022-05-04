using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using RookieOnlineAssetManagement.Entities;

namespace RookieOnlineAssetManagement.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CreateCategoryRequest request)
        {
            Regex sWhitespace = new Regex(@"\s+");
            var prefix = sWhitespace.Replace(request.Prefix, "");
            prefix = prefix.ToUpper();

            // check prefix exists
            var categoryPrefixExists = _context.Categories.Where(x => x.Prefix == prefix).FirstOrDefault();
            if (categoryPrefixExists != null)
            {
                return -1;
            }
            // check category name exists
            var categoryNameExists = _context.Categories.Where(x => x.CategoryName == request.CategoryName).FirstOrDefault();
            if (categoryNameExists != null)
            {
                return -2;
            }

            var category = new Category()
            {
                CategoryName = request.CategoryName,
                Prefix = prefix
            };
            _context.Categories.Add(category);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var categoryCreated = _context.Categories.Where(x => x.Prefix == prefix).FirstOrDefault();
                return categoryCreated.Id;
            }
            return 0;
        }

        public async Task<List<CategoryModel>> GetAll()
        {
            return await _context.Categories.Select(c => new CategoryModel()
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Prefix = c.Prefix
            }).ToListAsync();
        }

        public async Task<CategoryModel> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                var categoryVm = new CategoryModel()
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    Prefix = category.Prefix
                };
                return categoryVm;
            }
            return null;
        }
    }
}