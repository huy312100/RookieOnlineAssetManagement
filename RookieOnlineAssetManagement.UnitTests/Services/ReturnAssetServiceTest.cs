using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models.Common;
using RookieOnlineAssetManagement.Models.ReturnAssets;
using RookieOnlineAssetManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RookieOnlineAssetManagement.UnitTests.Services
{
    public class ReturnAssetServiceTest
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ReturnAssetService _returnAssetService;
        private readonly Mock<UserManager<User>> _mockUserManager;

        //Setup test and create fake data
        public ReturnAssetServiceTest()
        {
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _applicationDbContext = GetInmemoryContext();

            _returnAssetService = new ReturnAssetService(_applicationDbContext, _mockUserManager.Object);
            SeedDataDbTest();
        }

        ~ReturnAssetServiceTest()
        {
            _applicationDbContext.Dispose();
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
                FirstName = "Huy",
                LastName = "Nguyen",
                Email = "huy312100@gmail.com",
                BirthDay = new DateTime(2000, 10, 31),
                EmailConfirmed = true,
                NormalizedEmail = "huy312100@gmail.com",
                UserName = "huy312100",
                PasswordHash = hasher.HashPassword(null, "huy312100"),
                NormalizedUserName = "huy312100",
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
            _applicationDbContext.ReturnAssets.AddRange(
                new ReturnAsset()
                {
                    Id = 1,
                    UserId = adminHcmId,
                    AssignmentId = 1,
                    ReturnedDate = new DateTime(2022, 1, 3),
                    ModifiedDate = new DateTime(2022, 1, 3),
                    ReturnAssetState = Enums.ReturnAssetState.Completed
                },
                new ReturnAsset()
                {
                    Id = 2,
                    AssignmentId = 2,
                    ModifiedDate = new DateTime(2022, 1, 3),
                    ReturnAssetState = Enums.ReturnAssetState.WaitingForReturning
                },
                new ReturnAsset()
                {
                    Id = 3,
                    UserId = adminHcmId,
                    AssignmentId = 3,
                    ReturnedDate = new DateTime(2022, 1, 3),
                    ModifiedDate = new DateTime(2022, 1, 3),
                    ReturnAssetState = Enums.ReturnAssetState.Completed
                }
                );

            await _applicationDbContext.SaveChangesAsync();
        }

        private ApplicationDbContext GetInmemoryContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            return new ApplicationDbContext(builder.Options);
        }

        private GetReturnAssetPagingRequest GetPagingRequest()
        {
            return new GetReturnAssetPagingRequest
            {
                OrderMode = Enums.OrderMode.ASC,
                OrderName = "assetCode",
                PageIndex = 1,
                PageSize = 10,
            };
        }

        //Test method
        [Fact]
        public async Task GetAllPaging_ReturnPagedResultFailed_InValidAdminId()
        {
            // Arrage
            var request = GetPagingRequest();
            var pageResultFailed = new PagedResult<ReturnAssetModel>
            {
                TotalRecord = 0,
                Items = null,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());
            // Act
            var result = await _returnAssetService.GetAllPaging(It.IsAny<string>(), request);

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
            var data = new List<ReturnAssetModel>()
            {
                new ReturnAssetModel()
                {
                    Id=1,
                    AcceptedBy = "nghiangh",
                    AssetCode = "LA000001",
                    AssetName = "Laptop HP Probook 450 G1",
                    AssignedDate = new DateTime(2022, 1, 1),
                    RequestedBy = "tests",
                    ReturnedDate = new DateTime(2022, 1, 3),
                    ReturnAssetState = Enums.ReturnAssetState.Completed
                },
                new ReturnAssetModel()
                {
                    Id = 2,
                    AssetCode = "LA000001",
                    AssetName = "Laptop HP Probook 450 G1",
                    AssignedDate = new DateTime(2022, 1, 2),
                    RequestedBy = "tests",
                    ReturnAssetState = Enums.ReturnAssetState.WaitingForReturning
                },
                new ReturnAssetModel()
                {
                    Id = 3,
                    AcceptedBy = "nghiangh",
                    AssetCode = "LA000001",
                    AssetName = "Laptop HP Probook 450 G1",
                    AssignedDate = new DateTime(2022, 1, 3),
                    RequestedBy = "tests",
                    ReturnedDate = new DateTime(2022, 1, 3),
                    ReturnAssetState = Enums.ReturnAssetState.Completed
                },
            };
            var pageResultSuccess = new PagedResult<ReturnAssetModel>
            {
                TotalRecord = 3,
                Items = data.ToList(),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            var adminAccount = new User()
            {
                Id = "1C46E2DA-3B98-4B3F-8468-905EAF047338",
                FirstName = "admin",
                LastName = "nguyen",
                Gender = Enums.Gender.Male,
                UserName = "huy312100",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };
            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);
            // Act
            var result = await _returnAssetService.GetAllPaging(adminAccount.Id, request);

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
        public async Task CreateRequestReturnAssetUser_ReturnRequestReturnAsset_ValidAssignmentId()
        {
            // Arrage
            var adminHcmId = "EBEFE264-FE9C-4D25-B639-C7A554D43562";
            var staffHcmId = "8A669AA8-6774-40F9-AEAC-749BB5CC5952";
            var assignment = new Assignment()
            {
                Id = 1,
                AssignedDate = new DateTime(2022, 1, 1),
                AssetId = 1,
                AssignerId = adminHcmId,
                UserId = staffHcmId
            };
            var returnAsset = new ReturnAsset()
            {
                Id = 1,
                AssignmentId = assignment.Id,
                ModifiedDate = new DateTime(2022, 1, 3),
                ReturnAssetState = Enums.ReturnAssetState.WaitingForReturning
            };
            // Act
            var actualResult = await _returnAssetService.CreateRequestReturnAssetUser(assignment.Id);
            Assert.Equal(returnAsset.ReturnAssetState, actualResult.ReturnAssetState);
            Assert.Equal(returnAsset.UserId, actualResult.UserId);
        }
    }
}
