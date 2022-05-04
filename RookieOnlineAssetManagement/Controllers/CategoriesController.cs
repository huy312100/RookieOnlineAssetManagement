using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RookieOnlineAssetManagement.Models.Categories;
using RookieOnlineAssetManagement.Services;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return BadRequest($"Can not find category with id {id}");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryIdCreated = await _categoryService.Create(request);
            if (categoryIdCreated == -1)
            {
                return BadRequest("Prefix is already existed. Please enter a different prefix");
            }
            if (categoryIdCreated == -2)
            {
                return BadRequest("Category is already existed. Please enter a different category");
            }
            if (categoryIdCreated == 0)
            {
                return BadRequest("Create category failed");
            }
            var categoryCreated = await _categoryService.GetById(categoryIdCreated);
            return CreatedAtAction(nameof(GetById), new { id = categoryIdCreated, category = categoryCreated });
        }
    }
}