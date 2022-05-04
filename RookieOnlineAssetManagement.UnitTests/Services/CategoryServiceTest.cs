using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models.Assets;
using RookieOnlineAssetManagement.Models.Categories;
using RookieOnlineAssetManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RookieOnlineAssetManagement.UnitTests.Services
{
    public class CategoryServiceTest
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly CategoryService _categoryService;

        public CategoryServiceTest()
        {
            _applicationDbContext = GetInmemoryContext();

            _categoryService = new CategoryService(_applicationDbContext);
            SeedDataDbTest();
        }

        ~CategoryServiceTest()
        {
            _applicationDbContext.Dispose();
        }

        [Fact]
        public async Task Create_ReturnError_PrefixExistedError()
        {
            // Arrage
            var request = GetCreateCategoryRequest("Category Existed Test", "CE");

            var errorResultExpect = -1;

            // Act
            var errorResult = await _categoryService.Create(request);

            // Assert
            Assert.Equal(errorResultExpect, errorResult);
        }

        [Fact]
        public async Task Create_ReturnError_CategoryNameExistedError()
        {
            // Arrage
            var request = GetCreateCategoryRequest("Category Test 2", "CM");

            var errorResultExpect = -2;

            // Act
            var errorResult = await _categoryService.Create(request);

            // Assert
            Assert.Equal(errorResultExpect, errorResult);
        }

        [Fact]
        public async Task Create_ReturnCategoryId_CreateCategorySuccess()
        {
            // Arrage
            var request = GetCreateCategoryRequest("Category Test 3", "CC");

            var resultExpect = 3;

            // Act
            var actualResult = await _categoryService.Create(request);

            // Assert
            Assert.Equal(resultExpect, actualResult);
        }

        [Fact]
        public async Task GetById_ReturnNull_InvalidCategoryId()
        {
            // Arrage
            var categoryId = 4;

            // Act
            var actualResult = await _categoryService.GetById(categoryId);

            // Assert
            Assert.Null(actualResult);
        }

        [Fact]
        public async Task GetById_ReturnCategoryModel_ValidCategoryId()
        {
            // Arrage
            var categoryId = 1;
            var categoryExpect = new Category()
            {
                Id = 1,
                CategoryName = "Category Test 1",
                Prefix = "CT",
            };

            // Act
            var actualResult = await _categoryService.GetById(categoryId);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(categoryExpect.Id, actualResult.Id);
            Assert.Equal(categoryExpect.CategoryName, actualResult.CategoryName);
            Assert.Equal(categoryExpect.Prefix, actualResult.Prefix);
        }

        [Fact]
        public async Task GetAll_ReturnListCategoryModel_GetAllCategorySuccess()
        {
            // Arrage
            var categoriesExpect = new List<CategoryModel>()
            {
                new CategoryModel()
                {
                    Id = 1,
                    CategoryName = "Category Test 1",
                    Prefix = "CT",
                },
                new CategoryModel()
                {
                    Id = 2,
                    CategoryName = "Category Test 2",
                    Prefix = "CE",
                }
            };

            // Act
            var actualResult = await _categoryService.GetAll();

            // Assert
            Assert.NotNull(actualResult);
            for (var i = 0; i < actualResult.Count; i++)
            {
                Assert.Equal(categoriesExpect[i].Id, actualResult[i].Id);
                Assert.Equal(categoriesExpect[i].CategoryName, actualResult[i].CategoryName);
                Assert.Equal(categoriesExpect[i].Prefix, actualResult[i].Prefix);
            }
        }

        private ApplicationDbContext GetInmemoryContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            return new ApplicationDbContext(builder.Options);
        }

        private CreateCategoryRequest GetCreateCategoryRequest(string name, string prefix)
        {
            return new CreateCategoryRequest
            {
                CategoryName = name,
                Prefix = prefix
            };
        }

        private async void SeedDataDbTest()
        {
            _applicationDbContext.Categories.AddRange(
                new Category()
                {
                    Id = 1,
                    CategoryName = "Category Test 1",
                    Prefix = "CT",
                },
                new Category()
                {
                    Id = 2,
                    CategoryName = "Category Test 2",
                    Prefix = "CE",
                }
                );

            await _applicationDbContext.SaveChangesAsync();
        }
    }
}