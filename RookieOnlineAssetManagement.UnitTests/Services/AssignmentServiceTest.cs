using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Enums;
using RookieOnlineAssetManagement.Models.Assets;
using RookieOnlineAssetManagement.Models.Assignments;
using RookieOnlineAssetManagement.Models.Common;
using RookieOnlineAssetManagement.Services;
using RookieOnlineAssetManagement.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RookieOnlineAssetManagement.UnitTests.Services
{
    public class AssignmentServiceTest
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AssignmentService _assignmentService;
        private readonly AssetService _assetService;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public AssignmentServiceTest()
        {
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _applicationDbContext = GetInmemoryContext();

            _assignmentService = new AssignmentService(_applicationDbContext, _mockUserManager.Object);
            _assetService = new AssetService(_applicationDbContext, _mockUserManager.Object);
            SeedDataDbTest();
        }

        ~AssignmentServiceTest()
        {
            _applicationDbContext.Dispose();
        }

        [Fact]
        public async Task Create_ReturnError_PermissionDeniedError()
        {
            // Arrage
            var request = GetCreateAssignmentRequest("8A669AA8-6774-40F9-AEAC-749BB5CC5952", 1);

            var errorResultExpect = -1;
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());

            // Act
            var errorResult = await _assignmentService.Create(It.IsAny<string>(), request);

            // Assert
            Assert.Equal(errorResultExpect, errorResult);
        }

        [Fact]
        public async Task Create_ReturnError_InvalidUserIdError()
        {
            // Arrage
            var request = GetCreateAssignmentRequest("user-id-invalid", 1);
            var adminAccount = new User()
            {
                Id = "1C46E2DA-3B98-4B3F-8468-905EAF047338",
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };
            var errorResultExpect = -2;
            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);
            _mockUserManager.Setup(um => um.FindByIdAsync(request.UserId)).ReturnsAsync(It.IsAny<User>());

            // Act
            var errorResult = await _assignmentService.Create(adminAccount.Id, request);

            // Assert
            Assert.Equal(errorResultExpect, errorResult);
        }

        [Fact]
        public async Task Create_ReturnError_InvalidAssetIdError()
        {
            // Arrage
            var request = GetCreateAssignmentRequest("8A669AA8-6774-40F9-AEAC-749BB5CC5952", 10);
            var adminAccount = new User()
            {
                Id = "1C46E2DA-3B98-4B3F-8468-905EAF047338",
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };

            var userAssign = new User()
            {
                Id = "8A669AA8-6774-40F9-AEAC-749BB5CC5952",
                FirstName = "test",
                LastName = "staff",
                UserName = "tests",
                NormalizedUserName = "tests",
                LocationId = "hcm",
                Gender = Enums.Gender.Male,
                JoinedDate = DateTime.Now,
                IsNew = false
            };
            var errorResultExpect = -3;
            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);
            _mockUserManager.Setup(um => um.FindByIdAsync(request.UserId)).ReturnsAsync(userAssign);

            // Act
            var errorResult = await _assignmentService.Create(adminAccount.Id, request);

            // Assert
            Assert.Equal(errorResultExpect, errorResult);
        }

        [Fact]
        public async Task Create_ReturnAssignmentId_CreateAssignmentSuccess()
        {
            // Arrage
            var request = GetCreateAssignmentRequest("8A669AA8-6774-40F9-AEAC-749BB5CC5952", 1);
            var adminAccount = new User()
            {
                Id = "1C46E2DA-3B98-4B3F-8468-905EAF047338",
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };

            var userAssign = new User()
            {
                Id = "8A669AA8-6774-40F9-AEAC-749BB5CC5952",
                FirstName = "test",
                LastName = "staff",
                UserName = "tests",
                NormalizedUserName = "tests",
                LocationId = "hcm",
                Gender = Enums.Gender.Male,
                JoinedDate = DateTime.Now,
                IsNew = false
            };

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);
            _mockUserManager.Setup(um => um.FindByIdAsync(request.UserId)).ReturnsAsync(userAssign);

            // Act
            var actualResult = await _assignmentService.Create(adminAccount.Id, request);

            // Assert
            Assert.NotEqual(-3, actualResult);
            Assert.NotEqual(-2, actualResult);
            Assert.NotEqual(-1, actualResult);
            Assert.NotEqual(0, actualResult);
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultFailed_InValidAdminId()
        {
            // Arrage
            var request = GetPagingRequest();
            var pageResultFailed = new PagedResult<AssignmentModel>
            {
                TotalRecord = 0,
                Items = null,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());
            // Act
            var result = await _assignmentService.GetAllPaging(It.IsAny<string>(), request);

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
            var data = new List<AssignmentModel>()
            {
                new AssignmentModel()
                {
                    Id = 1,
                    AssignedTo = "tests",
                    AssetCode = "LA000001",
                    AssignedBy = "nghiangh"
                },
                new AssignmentModel()
                {
                    Id = 2,
                    AssignedTo = "tests",
                    AssetCode = "LA000001",
                    AssignedBy = "nghiangh"
                },
                new AssignmentModel()
                {
                    Id = 3,
                    AssignedTo = "tests",
                    AssetCode = "LA000001",
                    AssignedBy = "nghiangh"
                }
            };
            var pageResultSuccess = new PagedResult<AssignmentModel>
            {
                TotalRecord = data.Count,
                Items = data,
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
            var result = await _assignmentService.GetAllPaging(adminAccount.Id, request);

            // Assert
            Assert.Equal(pageResultSuccess.TotalRecord, result.TotalRecord);
            Assert.Equal(pageResultSuccess.PageIndex, result.PageIndex);
            Assert.Equal(pageResultSuccess.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(pageResultSuccess.Items[i].Id, result.Items[i].Id);
                Assert.Equal(pageResultSuccess.Items[i].AssignedBy, result.Items[i].AssignedBy);
                Assert.Equal(pageResultSuccess.Items[i].AssignedTo, result.Items[i].AssignedTo);
                Assert.Equal(pageResultSuccess.Items[i].AssetCode, result.Items[i].AssetCode);
            }
        }

        [Fact]
        public async Task DeleteAssignment_ReturnAssignment_ValidAssignmentId()
        {
            // Arrage
            var adminHcmId = "EBEFE264-FE9C-4D25-B639-C7A554D43562";
            var staffHcmId = "8A669AA8-6774-40F9-AEAC-749BB5CC5952";
            var hasher = new PasswordHasher<User>();
            var adminAccount = new User()
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
            };

            var userAssign = new User()
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
            };
            var assignmentDelete = new Assignment()
            {
                Id = 3,
                AssignedDate = new DateTime(2022, 1, 3),
                AssetId = 1,
                AssignerId = adminHcmId,
                UserId = staffHcmId
            };
            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);
            _mockUserManager.Setup(um => um.FindByIdAsync(userAssign.Id)).ReturnsAsync(userAssign);

            // Act
            var actualResult = await _assignmentService.DeleteAssignment(adminAccount.Id, assignmentDelete.Id);

            Assert.Equal(assignmentDelete.Id, actualResult.Id);
        }

        [Fact]
        public async Task DeleteAssignment_ReturnNull_InValidAssignmentId()
        {
            // Arrage
            var adminHcmId = "EBEFE264-FE9C-4D25-B639-C7A554D43562";
            var staffHcmId = "8A669AA8-6774-40F9-AEAC-749BB5CC5952";
            var hasher = new PasswordHasher<User>();
            var adminAccount = new User()
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
            };

            var userAssign = new User()
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
            };
            var assignmentDelete = new Assignment()
            {
                Id = 3,
                AssignedDate = new DateTime(2022, 1, 3),
                AssetId = 1,
                AssignerId = adminHcmId,
                UserId = staffHcmId
            };
            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);
            _mockUserManager.Setup(um => um.FindByIdAsync(userAssign.Id)).ReturnsAsync(userAssign);

            // Act
            var actualResult = await _assignmentService.DeleteAssignment(adminAccount.Id, assignmentDelete.Id);

            Assert.Equal(assignmentDelete.Id, actualResult.Id);
        }

        [Fact]
        public async Task Update_ReturnMessageSuccess_ValidInformation()
        {
            // Arrage
            var request = GetUpdateAssignmentRequest();
            var assignmentId = 1;
            var resExpected = SystemConstants.APIUpdateAssignmentReturnSuccess;

            // Act
            var result = await _assignmentService.Update(assignmentId, request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(resExpected, result);
        }

        [Fact]
        public async Task Update_ReturnMessageSuccess_InvalidAssignmentId()
        {
            // Arrage
            var request = GetUpdateAssignmentRequest();
            var assignmentId = 999;
            var resExpected = SystemConstants.APIFindAssignmentByIdErrorInValidId;

            // Act
            var result = await _assignmentService.Update(assignmentId, request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(resExpected, result);
        }

        [Fact]
        public async Task Update_ReturnMessageSuccess_InvalidUserId()
        {
            // Arrage
            var request = new UpdateAssignmentRequest
            {
                UserId = "913f1623-4fe6-41c9-bdec-84e8fa4c56bb",
                NewAssetId = new Random().Next(1,100),
                OldAssetId = new Random().Next(1,100),
                AssignedDate = DateTime.Now,
                Note = "lorem ipsum dolor sit amet",
            }; 
            var assignmentId = 1;
            var resExpected = SystemConstants.APIFindUserByIdErrorInValidId;

            // Act
            var result = await _assignmentService.Update(assignmentId, request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(resExpected, result);
        }

        [Fact]
        public async Task Update_ReturnMessageSuccess_InvalidAssetId()
        {
            // Arrage
            var request = new UpdateAssignmentRequest
            {
                UserId = "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                NewAssetId = 999,
                OldAssetId = 1,
                AssignedDate = DateTime.Now,
                Note = "lorem ipsum dolor sit amet",
            };
            var assignmentId = 1;
            var resExpected = SystemConstants.APIFindAssetByIdErrorInValidId;

            // Act
            var result = await _assignmentService.Update(assignmentId, request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(resExpected, result);
        }

        [Fact]
        public async Task Update_ReturnNewAssetStateIsAssignedAfterUpdated_CorrectInfomation()
        {
            // Arrage
            var request = GetUpdateAssignmentRequest();
            var assignmentId = 1;
            var resExpected = Enums.AssetState.Assigned;

            // Act
            await _assignmentService.Update(assignmentId, request);

            var result = await _assetService.GetById(request.NewAssetId);

            // Assert
            Assert.Equal(resExpected, result.AssetState);
        }

        [Fact]
        public async Task Update_ReturnOldAssetStateIsAvailableAfterUpdated_CorrectInfomation()
        {
            // Arrage
            var request = GetUpdateAssignmentRequest();
            var assignmentId = 1;
            var resExpected = Enums.AssetState.Available;

            // Act
            await _assignmentService.Update(assignmentId, request);
            var result = await _assetService.GetById(request.OldAssetId);

            // Assert
            Assert.Equal(resExpected, result.AssetState);
        }

        //[Fact]
        //public async Task Update_ReturnMessageError_UpdateToDbFail()
        //{
        //    // Arrage
        //    var request = GetUpdateAssignmentRequest();
        //    var assetId = 999;
        //     var resExpected = SystemConstants.APIUpdateAssignmentReturnError;

        //    var _mockDbContext = new Mock<ApplicationDbContext>();
        //    _mockDbContext.Setup(a => a.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(-1);
        //    _mockDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None));

        //    // Act
        //    var result = await _assignmentService.Update(assetId, request);

        //    // Assert
        //    Assert.NotEmpty(result);
        //    Assert.Equal(resExpected, result);
        //}


        private GetAssignmentPagingRequest GetPagingRequest()
        {
            return new GetAssignmentPagingRequest
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

        private CreateAssignmentRequest GetCreateAssignmentRequest(string userId, int assetId)
        {
            return new CreateAssignmentRequest
            {
                AssetId = assetId,
                AssignedDate = new DateTime(2022, 4, 15),
                UserId = userId,
                Note = "test1"
            };
        }

        private UpdateAssignmentRequest GetUpdateAssignmentRequest()
        {
            return new UpdateAssignmentRequest
            {
                UserId = "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                NewAssetId = 3,
                OldAssetId = 1,
                AssignedDate = DateTime.Now,
                Note = "lorem ipsum dolor sit amet",
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