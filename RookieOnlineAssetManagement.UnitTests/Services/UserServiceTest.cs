using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models;
using RookieOnlineAssetManagement.Models.Common;
using RookieOnlineAssetManagement.Models.Users;
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
    public class UserServiceTest
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserService _userService;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public UserServiceTest()
        {
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            _applicationDbContext = GetInmemoryContext();

            _userService = new UserService(_applicationDbContext, _mockUserManager.Object);
            SeedDataDbTest();
        }

        ~UserServiceTest()
        {
            _applicationDbContext.Dispose();
        }

        [Fact]
        public async Task Create_ReturnUserId_UserCreateSuccess()
        {
            // Arrage
            var request = GetCreateUserRequest();
            var userInsert = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDay = request.BirthDay,
                JoinedDate = request.JoinedDate,
                Gender = request.Gender,
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };
            var adminAccount = new User()
            {
                Id = "1C46E2DA-3B98-4B3F-8468-905EAF047338",
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDay = request.BirthDay,
                JoinedDate = request.JoinedDate,
                Gender = request.Gender,
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };
            var userIdExpect = "1C46E2DA-3B98-4B3F-8468-905EAF047338";
            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);
            var userManagerCreateResult = new IdentityResult();
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userInsert.Id = userIdExpect;
            _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(userInsert);
            _mockUserManager.Setup(um => um.AddToRoleAsync(userInsert, request.Type)).ReturnsAsync(IdentityResult.Success);
            // Act
            var userIdCreated = await _userService.Create(adminAccount.Id, request);

            // Assert
            Assert.NotEmpty(userIdCreated);
            Assert.Equal(userIdExpect, userIdCreated);
        }

        [Fact]
        public async Task Create_ReturnMessageError_PermissionDeniedError()
        {
            // Arrage
            var request = GetCreateUserRequest();
            var userInsert = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDay = request.BirthDay,
                JoinedDate = request.JoinedDate,
                Gender = request.Gender,
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };
            var messageErrorExpect = SystemConstants.APIReturnPermissionDenied;
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());

            // Act
            var messageError = await _userService.Create(It.IsAny<string>(), request);

            // Assert
            Assert.NotEmpty(messageError);
            Assert.Equal(messageErrorExpect, messageError);
        }

        [Fact]
        public async Task Create_ReturnMessageError_UserCreateError()
        {
            // Arrage
            var request = GetCreateUserRequest();
            var userInsert = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDay = request.BirthDay,
                JoinedDate = request.JoinedDate,
                Gender = request.Gender,
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };
            var adminAccount = new User()
            {
                Id = "1C46E2DA-3B98-4B3F-8468-905EAF047338",
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDay = request.BirthDay,
                JoinedDate = request.JoinedDate,
                Gender = request.Gender,
                UserName = "nghiangh",
                LocationId = "hcm",
                Status = Enums.Status.Active
            };
            var messageErrorExpect = "Create User Failed";
            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            // Act
            var messageError = await _userService.Create(adminAccount.Id, request);

            // Assert
            Assert.NotEmpty(messageError);
            Assert.Equal(messageErrorExpect, messageError);
        }

        [Fact]
        public async Task GetById_ReturnUserModel_ValidUserId()
        {
            // Arrage
            var request = GetCreateUserRequest();
            var userId = "D528E31D-3134-4BB1-B5A9-A287AA1696C0";
            var userExpect = new User()
            {
                Id = userId,
                BirthDay = new DateTime(1999, 06, 08),
                FirstName = "Nghia",
                LastName = "Nguyen Gia Huu",
                Gender = Enums.Gender.Male,
                JoinedDate = DateTime.Now,
                LocationId = "hn",
                UserName = "nghiangh",
                Serial = 1,
                Status = Enums.Status.Active
            };
            var roles = new List<string>();

            roles.Add("Staff");

            _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(userExpect);
            _mockUserManager.Setup(um => um.GetRolesAsync(userExpect)).ReturnsAsync(roles);
            // Act
            var userResult = await _userService.GetUserById(userId);

            // Assert
            Assert.NotNull(userResult);
            Assert.Equal(userResult.UserName, userExpect.UserName);
            Assert.Equal(userResult.Id, userExpect.Id);
            Assert.Equal(userResult.JoinedDate, userExpect.JoinedDate);
            Assert.Equal(userResult.Gender, userExpect.Gender);
            Assert.Equal(userResult.BirthDay, userExpect.BirthDay);
            Assert.Equal("Staff", userResult.Type);
        }

        [Fact]
        public async Task GetById_ReturnNull_InValidUserId()
        {
            // Arrage
            var request = GetCreateUserRequest();
            User userExpect = null;
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userExpect);
            // Act
            var userResult = await _userService.GetUserById(It.IsAny<string>());

            // Assert
            Assert.Null(userResult);
        }

        [Fact]
        public async Task Update_ReturnMessageError_InValidUserId()
        {
            // Arrage
            var request = GetUpdateUserRequest();
            User userExpect = null;
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userExpect);
            // Act
            var result = await _userService.Update(It.IsAny<string>(), request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(SystemConstants.APIFindUserByIdErrorInValidId, result);
        }

        [Fact]
        public async Task Update_ReturnMessageError_UpdateInfoUserFailed()
        {
            // Arrage
            var request = GetUpdateUserRequest();
            User userExpect = new User()
            {
                Id = "DC7BE400-528D-459B-BE5C-5140F58FF903",
                BirthDay = new DateTime(2000, 06, 08),
                Gender = Enums.Gender.Female,
                JoinedDate = new DateTime(2022, 01, 08)
            };
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userExpect);
            _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _userService.Update(It.IsAny<string>(), request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(SystemConstants.APIUpdateUserReturnError, result);
        }

        [Fact]
        public async Task Update_ReturnMessageError_RomoveOldRoleFailed()
        {
            // Arrage
            var request = GetUpdateUserRequest();
            User userExpect = new User()
            {
                Id = "DC7BE400-528D-459B-BE5C-5140F58FF903",
                BirthDay = new DateTime(2000, 06, 08),
                Gender = Enums.Gender.Female,
                JoinedDate = new DateTime(2022, 01, 08)
            };
            var oldRole = "Staff";
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userExpect);
            _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { oldRole });
            _mockUserManager.Setup(um => um.RemoveFromRoleAsync(It.IsAny<User>(), oldRole)).ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _userService.Update(It.IsAny<string>(), request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(SystemConstants.APIUpdateUserReturnError, result);
        }

        [Fact]
        public async Task Update_ReturnMessageError_AddNewRoleFailed()
        {
            // Arrage
            var request = GetUpdateUserRequest();
            User userExpect = new User()
            {
                Id = "DC7BE400-528D-459B-BE5C-5140F58FF903",
                BirthDay = new DateTime(2000, 06, 08),
                Gender = Enums.Gender.Female,
                JoinedDate = new DateTime(2022, 01, 08)
            };
            var oldRole = "Staff";
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userExpect);
            _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { oldRole });
            _mockUserManager.Setup(um => um.RemoveFromRoleAsync(It.IsAny<User>(), oldRole)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), request.Type)).ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _userService.Update(It.IsAny<string>(), request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(SystemConstants.APIUpdateUserReturnError, result);
        }

        [Fact]
        public async Task Update_ReturnMessageSuccess_ValidUserId()
        {
            // Arrage
            var request = GetUpdateUserRequest();
            User userExpect = new User()
            {
                Id = "DC7BE400-528D-459B-BE5C-5140F58FF903",
                BirthDay = new DateTime(2000, 06, 08),
                Gender = Enums.Gender.Female,
                JoinedDate = new DateTime(2022, 01, 08)
            };
            var oldRole = "Staff";
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userExpect);
            _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { oldRole });
            _mockUserManager.Setup(um => um.RemoveFromRoleAsync(It.IsAny<User>(), oldRole)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), request.Type)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.Update(It.IsAny<string>(), request);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(SystemConstants.APIUpdateUserReturnSuccess, result);
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultFailed_InValidAdminId()
        {
            // Arrage
            var request = GetPagingRequest();
            var pageResultFailed = new PagedResult<UserModel>
            {
                TotalRecord = 0,
                Items = new List<UserModel>(),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());
            // Act
            var result = await _userService.GetAllPaging(It.IsAny<string>(), request);

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
            var data = new List<UserModel>()
            {
                new UserModel()
                {
                    Id = "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                    FullName = "Nghia Nguyen Gia Huu",
                    Gender = Enums.Gender.Male,
                    UserName = "nghiangh"
                },
                new UserModel()
                {
                    Id = "8A669AA8-6774-40F9-AEAC-749BB5CC5952",
                    FullName = "test staff",
                    Gender = Enums.Gender.Male,
                    UserName = "tests"
                }
            };
            var pageResultSuccess = new PagedResult<UserModel>
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
            var result = await _userService.GetAllPaging(adminAccount.Id, request);

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
        public async Task DisableUser_ReturnUser_ValidUsername()
        {
            var staffHcmId = "8A669AA8-6774-40F9-AEAC-749BB5CC5952";
            var hasher = new PasswordHasher<User>();
            User userDisable = new User
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
            _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(userDisable);
            
            var result = await _userService.DisableUser(userDisable.UserName);

            Assert.Equal(userDisable, result);
        }
        [Fact]
        public async Task DisableUser_ReturnNull_InValidUsername()
        {            
            User userExpect = null;
            _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(userExpect);

            var result = await _userService.DisableUser(It.IsAny<string>());

            Assert.Equal(userExpect, result);
        }
        private ApplicationDbContext GetInmemoryContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("TestDatabase");

            return new ApplicationDbContext(builder.Options);
        }

        private CreateUserRequest GetCreateUserRequest()
        {
            return new CreateUserRequest
            {
                FirstName = "Nghia",
                LastName = "Nguyen Gia Huu",
                BirthDay = new DateTime(1999, 06, 08),
                Gender = Enums.Gender.Male,
                JoinedDate = DateTime.Now,
                Type = "staff"
            };
        }


        private GetUserPagingRequest GetPagingRequest()
        {
            return new GetUserPagingRequest
            {
                PageIndex = 1,
                PageSize = 10,
                Keyword = "",
                Type = "",
                OrderName = "",
                OrderMode = Enums.OrderMode.ASC
            };
        }

        private UpdateUserRequest GetUpdateUserRequest()
        {
            return new UpdateUserRequest
            {
                BirthDay = new DateTime(2000, 06, 08),
                Gender = Enums.Gender.Female,
                JoinedDate = new DateTime(2022, 01, 08),
                Type = "admin"
            };
        }

        private async void SeedDataDbTest()
        {
            _applicationDbContext.Locations.AddRange(new Location() { Id = "hcm", LocationName = "Hồ Chí Minh" },
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
            var adminHnId = "369364B3-0EBA-40E3-B72B-ED8CB3424B2F";
            _applicationDbContext.Users.Add(new User()
            {
                Id = adminHnId,
                FirstName = "Khang",
                LastName = "Nguyen Duc",
                Email = "nguyenduckhang@gmail.com",
                BirthDay = new DateTime(2000, 10, 31),
                EmailConfirmed = true,
                NormalizedEmail = "nguyenduckhang@gmail.com",
                UserName = "khangnd",
                PasswordHash = hasher.HashPassword(null, "khangnd@31102000"),
                NormalizedUserName = "khangnd",
                LocationId = "hn",
                Gender = Enums.Gender.Male,
                JoinedDate = DateTime.Now,
                IsNew = false
            });
            _applicationDbContext.UserRoles.Add(new IdentityUserRole<string> { RoleId = roleAdminId, UserId = adminHnId });
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
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}