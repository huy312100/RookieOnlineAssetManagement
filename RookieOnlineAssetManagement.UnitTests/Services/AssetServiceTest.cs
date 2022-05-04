using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models.Assets;
using RookieOnlineAssetManagement.Models.Common;
using RookieOnlineAssetManagement.Services;
using RookieOnlineAssetManagement.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RookieOnlineAssetManagement.UnitTests.Services
{
    public class AssetServiceTest
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AssetService _assetService;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public AssetServiceTest()
        {
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _applicationDbContext = GetInmemoryContext();

            _assetService = new AssetService(_applicationDbContext, _mockUserManager.Object);
            SeedDataDbTest();
        }

        ~AssetServiceTest()
        {
            _applicationDbContext.Dispose();
        }

        [Fact]
        public async Task Create_ReturnError_PermissionDeniedError()
        {
            // Arrage
            var request = GetCreateAssetRequest(1);

            var errorResultExpect = -1;
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());

            // Act
            var errorResult = await _assetService.Create(It.IsAny<string>(), request);

            // Assert
            Assert.Equal(errorResultExpect, errorResult);
        }

        [Fact]
        public async Task Create_ReturnError_InvalidCategoryError()
        {
            // Arrage
            var request = GetCreateAssetRequest(3);
            var adminAccount = new User()
            {
                Id = "1C46E2DA-3B98-4B3F-8468-905EAF047338",
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };
            var errorResultExpect = -2;
            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            // Act
            var errorResult = await _assetService.Create(adminAccount.Id, request);

            // Assert
            Assert.Equal(errorResultExpect, errorResult);
        }

        [Fact]
        public async Task Create_ReturnAssetId_CreateAssetSuccess()
        {
            // Arrage
            var request = GetCreateAssetRequest(1);
            var adminAccount = new User()
            {
                Id = "1C46E2DA-3B98-4B3F-8468-905EAF047338",
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            // Act
            var result = await _assetService.Create(adminAccount.Id, request);

            // Assert
            Assert.NotEqual(-2, result);
            Assert.NotEqual(-1, result);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultFailed_InValidAdminId()
        {
            // Arrage
            var request = GetPagingRequest();
            var pageResultFailed = new PagedResult<AssetModel>
            {
                TotalRecord = 0,
                Items = null,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());
            // Act
            var result = await _assetService.GetAllPaging(It.IsAny<string>(), request);

            // Assert
            Assert.Equal(pageResultFailed.TotalRecord, result.TotalRecord);
            Assert.Equal(pageResultFailed.PageIndex, result.PageIndex);
            Assert.Equal(pageResultFailed.PageSize, result.PageSize);
            Assert.Equal(pageResultFailed.Items, result.Items);
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultSuccess_ValidAdminId()
        {
            // Arrage
            var request = GetPagingRequest();
            var data = new List<AssetModel>()
            {
                new AssetModel()
                {
                    AssetCode = "LA000001",
                    AssetName = "Laptop HP Probook 450 G1",
                    AssetState = Enums.AssetState.Available,
                    LocationId = "hcm",
                    Id = 1
                },
                new AssetModel()
                {
                    AssetCode = "LA000002",
                    AssetName = "Laptop HP Probook 450 G2",
                    AssetState = Enums.AssetState.Available,
                    LocationId = "hn",
                    Id = 2
                },
                new AssetModel()
                {
                    AssetCode = "MO000001",
                    AssetName = "Monitor HP Probook 450 G1",
                    AssetState = Enums.AssetState.Available,
                    LocationId = "hcm",
                    Id = 3
                },
            };
            var pageResultSuccess = new PagedResult<AssetModel>
            {
                TotalRecord = 2,
                Items = data.Where(x => x.LocationId == "hcm").ToList(),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            var adminAccount = new User()
            {
                Id = "1C46E2DA-3B98-4B3F-8468-905EAF047338",
                FirstName = "admin",
                LastName = "nguyen",
                Gender = Enums.Gender.Male,
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };
            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);
            // Act
            var result = await _assetService.GetAllPaging(adminAccount.Id, request);

            // Assert
            Assert.Equal(pageResultSuccess.TotalRecord, result.TotalRecord);
            Assert.Equal(pageResultSuccess.PageIndex, result.PageIndex);
            Assert.Equal(pageResultSuccess.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(pageResultSuccess.Items[i].Id, result.Items[i].Id);
            }
        }

        [Fact]
        public async Task GetAssetHistoryPaging_ReturnPagedResultFailed_InValidAssetId()
        {
            // Arrage
            var request = new PagingRequestBase()
            {
                OrderMode = Enums.OrderMode.ASC,
                OrderName = "",
                PageIndex = 1,
                PageSize = 10
            };
            var pageResultFailed = new PagedResult<AssetHistoryModel>
            {
                TotalRecord = 0,
                Items = null,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            var assetIdInvalid = 10;
            // Act
            var result = await _assetService.GetAssetHistoryPaging(assetIdInvalid, request);

            // Assert
            Assert.Equal(pageResultFailed.TotalRecord, result.TotalRecord);
            Assert.Equal(pageResultFailed.PageIndex, result.PageIndex);
            Assert.Equal(pageResultFailed.PageSize, result.PageSize);
            Assert.Null(result.Items);
        }

        [Fact]
        public async Task GetAssetHistoryPaging_ReturnPagedResultSuccess_ValidAssetId()
        {
            // Arrage
            var request = new PagingRequestBase()
            {
                OrderMode = Enums.OrderMode.ASC,
                OrderName = "",
                PageIndex = 1,
                PageSize = 10
            };
            var data = new List<AssetHistoryModel>()
            {
                new AssetHistoryModel()
                {
                    AssignedBy = "nghiangh",
                    AssignedDate = new DateTime(2022, 1,1),
                    AssignedTo = "tests",
                    ReturnedDate = null
                },
                new AssetHistoryModel()
                {
                    AssignedBy = "nghiangh",
                    AssignedDate = new DateTime(2022, 1,2),
                    AssignedTo = "tests",
                    ReturnedDate = null
                },
                new AssetHistoryModel()
                {
                    AssignedBy = "nghiangh",
                    AssignedDate = new DateTime(2022, 1,3),
                    AssignedTo = "tests",
                    ReturnedDate = null
                },
            };

            var pageResultSuccess = new PagedResult<AssetHistoryModel>
            {
                TotalRecord = 3,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            var assetIdTest = 1;
            // Act
            var result = await _assetService.GetAssetHistoryPaging(assetIdTest, request);

            // Assert
            Assert.Equal(pageResultSuccess.TotalRecord, result.TotalRecord);
            Assert.Equal(pageResultSuccess.PageIndex, result.PageIndex);
            Assert.Equal(pageResultSuccess.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(pageResultSuccess.Items[i].AssignedTo, result.Items[i].AssignedTo);
                Assert.Equal(pageResultSuccess.Items[i].AssignedBy, result.Items[i].AssignedBy);
            }
        }
        [Fact]
        public async Task DeleteAsset_ReturnAsset_ValidAssetId()
        {
            var assetId = 1;
            Asset assetDelete = new Asset
            {
                AssetCode = "LA000001",
                AssetName = "Laptop HP Probook 450 G1",
                AssetState = Enums.AssetState.Available,
                LocationId = "hcm",
                Id = 1,
                CategoryId = 1,
            };
            var result = await _assetService.DeleteAsset(assetId);
            Assert.Equal(assetDelete.AssetCode, result.AssetCode);
        }

        [Fact]
        public async Task DeleteAsset_ReturnNull_InValidAssetId()
        {
            Asset assetExpect = null;
            var result = await _assetService.DeleteAsset(It.IsAny<int>());
            Assert.Equal(assetExpect, result);
        }

        [Fact]
        public async Task Update_ReturnMessageError_InValidAssetId()
        {
            // Arrage
            var request = GetUpdateAssetRequest();
            var assetId = 999;
            var resExpected = SystemConstants.APIFindAssetByIdErrorInValidId;

            // Act
            var result = await _assetService.Update(assetId, request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(resExpected, result);
        }

        //[Fact]
        //public async Task Update_ReturnMessageError_UpdateToDbFail()
        //{
        //    // Arrage
        //    var request = GetUpdateAssetRequest();
        //    var assetId = 999;
        //    var resExpected = SystemConstants.APIUpdateAssetReturnError;

        //    var _mockDbContext = new Mock<ApplicationDbContext>();
        //    _mockDbContext.Setup(a => a.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(-1);
        //    _mockDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None));

        //    // Act
        //    var result = await _assetService.Update(assetId, request);

        //    // Assert
        //    Assert.NotEmpty(result);
        //    Assert.Equal(resExpected, result);
        //}

        [Fact]
        public async Task Update_ReturnMessageSuccess_ValidAssetId()
        {
            // Arrage
            var request = GetUpdateAssetRequest();
            var assetId = 1;
            var resExpected = SystemConstants.APIUpdateAssetReturnSuccess;

            // Act
            var result = await _assetService.Update(assetId, request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(resExpected, result);
        }

        private GetAssetPagingRequest GetPagingRequest()
        {
            return new GetAssetPagingRequest
            {
                OrderMode = Enums.OrderMode.ASC,
                OrderName = "assetCode",
                PageIndex = 1,
                PageSize = 10,
            };
        }

        private ApplicationDbContext GetInmemoryContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            return new ApplicationDbContext(builder.Options);
        }

        private CreateAssetRequest GetCreateAssetRequest(int categoryId)
        {
            return new CreateAssetRequest
            {
                AssetName = "TestAsset",
                AssetState = Enums.AssetState.Available,
                CategoryId = categoryId,
                InstalledDate = DateTime.Now,
                Specification = "Test Specification"
            };
        }

        private UpdateAssetRequest GetUpdateAssetRequest()
        {
            return new UpdateAssetRequest
            {
                AssetName = "TestAsset",
                InstalledDate = DateTime.Now,
                Specification = "Test Specification",
                AssetState = Enums.AssetState.WaitingForRecycling,
            };
        }

        private async void SeedDataDbTest()
        {
            _applicationDbContext.Locations.AddRange(
                new Location() { Id = "hcm", LocationName = "Hồ Chí Minh" },
                new Location() { Id = "hn", LocationName = "Hà Nội" });
            var roleAdminId = "354C4368-A6DC-43FA-8627-506C21DEF50E";
            var roleStaffId = "4DB16EF4-4E54-46EA-B7CB-34892D60DF33";
            var adminHcmId = "EBEFE264-FE9C-4D25-B639-C7A554D43562";
            _applicationDbContext.Roles.AddRange(new Role() { Id = roleAdminId, Name = "Admin", NormalizedName = "admin", Description = "Adminstrator role" },
                new Role() { Id = roleStaffId, Name = "Staff", NormalizedName = "staff", Description = "Staff role" });
            var hasher = new PasswordHasher<User>();
            _applicationDbContext.Users.Add(new User()
            {
                Id = adminHcmId,
                FirstName = "Nghia",
                LastName = "Nguyen Gia Huu",
                Email = "nguyengiahuunghia118@gmail.com",
                BirthDay = new DateTime(1999, 6, 8),
                EmailConfirmed = true,
                NormalizedEmail = "nguyengiahuunghia118@gmail.com",
                UserName = "nghiangh",
                PasswordHash = hasher.HashPassword(null, "nghiangh@08061999"),
                NormalizedUserName = "nghiangh",
                LocationId = "hcm",
                Gender = Enums.Gender.Male,
                JoinedDate = DateTime.Now,
                IsNew = false
            });
            _applicationDbContext.UserRoles.Add(new IdentityUserRole<string> { RoleId = roleAdminId, UserId = adminHcmId });

            var staffHcmId = "8A669AA8-6774-40F9-AEAC-749BB5CC5952";
            _applicationDbContext.Users.Add(new User()
            {
                Id = staffHcmId,
                FirstName = "test",
                LastName = "staff",
                UserName = "tests",
                PasswordHash = hasher.HashPassword(null, "tests@12121999"),
                NormalizedUserName = "tests",
                LocationId = "hcm",
                Gender = Enums.Gender.Male,
                JoinedDate = DateTime.Now,
                IsNew = false
            });
            _applicationDbContext.UserRoles.Add(new IdentityUserRole<string> { RoleId = roleStaffId, UserId = staffHcmId });
            _applicationDbContext.Categories.AddRange(
                new Category()
                {
                    Id = 1,
                    CategoryName = "Category Test 1",
                    Prefix = "C1",
                },
                new Category()
                {
                    Id = 2,
                    CategoryName = "Category Test 2",
                    Prefix = "C2",
                }
                );
            _applicationDbContext.Assets.AddRange(
                new Asset()
                {
                    AssetCode = "LA000001",
                    AssetName = "Laptop HP Probook 450 G1",
                    AssetState = Enums.AssetState.Available,
                    LocationId = "hcm",
                    Id = 1,
                    CategoryId = 1,
                },
                new Asset()
                {
                    AssetCode = "LA000002",
                    AssetName = "Laptop HP Probook 450 G2",
                    AssetState = Enums.AssetState.Available,
                    LocationId = "hn",
                    Id = 2,
                    CategoryId = 1
                },
                new Asset()
                {
                    AssetCode = "MO000001",
                    AssetName = "Monitor HP Probook 450 G1",
                    AssetState = Enums.AssetState.Available,
                    LocationId = "hcm",
                    Id = 3,
                    CategoryId = 1
                }
                );
            _applicationDbContext.Assignments.AddRange(
                new Assignment()
                {
                    Id = 1,
                    AssignedDate = new DateTime(2022, 1, 1),
                    AssetId = 1,
                    AssignerId = adminHcmId,
                    UserId = staffHcmId
                },
                new Assignment()
                {
                    Id = 2,
                    AssignedDate = new DateTime(2022, 1, 2),
                    AssetId = 1,
                    AssignerId = adminHcmId,
                    UserId = staffHcmId
                },
                new Assignment()
                {
                    Id = 3,
                    AssignedDate = new DateTime(2022, 1, 3),
                    AssetId = 1,
                    AssignerId = adminHcmId,
                    UserId = staffHcmId
                },
                new Assignment()
                {
                    Id = 4,
                    AssignedDate = new DateTime(2022, 1, 3),
                    AssetId = 2,
                    AssignerId = adminHcmId,
                    UserId = staffHcmId
                }
                );

            await _applicationDbContext.SaveChangesAsync();
        }
    }
}