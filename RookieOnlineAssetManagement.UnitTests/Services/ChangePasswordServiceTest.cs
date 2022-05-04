using System;
using Microsoft.AspNetCore.Identity;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Services;
using Xunit;
using Moq;
using RookieOnlineAssetManagement.Entities;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Models.Users;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.UnitTests.ServiceTest
{
    public class ChangePasswordServiceTest
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserService _userService;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public ChangePasswordServiceTest()
        {
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;

            var hasher = new PasswordHasher<User>();

            using (var context = new ApplicationDbContext(options))
            {
                context.Users.AddRange(
                    new User
                    {
                        Id = "28131807-0845-4bdb-8b35-e0c6a371c27b",
                        FirstName = "Huy",
                        LastName = "Nguyen",
                        BirthDay = new DateTime(1999, 03, 21),
                        Gender = Enums.Gender.Male,
                        JoinedDate = DateTime.Now,
                        LocationId = "hcm",
                        PasswordHash = hasher.HashPassword(null, "PasswordInDB"),
                        IsNew =  false
                    });

                context.Users.AddRange(
                    new User
                    {
                        Id = "AD6E8761-67C0-45AB-9EB6-0AE98EB888B1",
                        FirstName = "Khang",
                        LastName = "Nguyen",
                        BirthDay = new DateTime(2000, 03, 22),
                        Gender = Enums.Gender.Female,
                        JoinedDate = DateTime.Now,
                        LocationId = "hn",
                        PasswordHash = hasher.HashPassword(null, "Ndkhang3110"),
                        IsNew = true
                    });

                context.SaveChanges();
            }

            var mockContext = new ApplicationDbContext(options);
            _userService = new UserService(mockContext, _mockUserManager.Object);
        }

        private ChangePasswordRequest GetChangePasswordRequest()
        {
            return new ChangePasswordRequest
            {
                UserId = "28131807-0845-4bdb-8b35-e0c6a371c27b",
                PasswordOld = "OldPassword",
                PasswordNew = "NewPassword",
            };
        }

        private ChangePasswordFirstTimeModel GetChangePasswordRequestFirstTime()
        {
            return new ChangePasswordFirstTimeModel
            {
                UserId = "28131807-0845-4bdb-8b35-e0c6a371c27b",
                PasswordNew = "NewPassword",
            };
        }

        [Fact]
        public async Task ChangePassword_Return0_UseridIsNull()
        {
            // Arrage
            var request = GetChangePasswordRequest();
            var changePasswordModel = new ChangePasswordRequest()
            {
                UserId = null,
                PasswordOld = request.PasswordOld,
                PasswordNew = request.PasswordNew
            };

            var expectedRes = 0;

            //Act
            var res = await _userService.ChangePassword(changePasswordModel);

            //Assert
            Assert.Equal(expectedRes, res);
        }

        [Fact]
        public async Task ChangePassword_Return0_OldPasswordIsEmpty()
        {
            // Arrage
            var request = GetChangePasswordRequest();
            var changePasswordModel = new ChangePasswordRequest()
            {
                UserId = request.UserId,
                PasswordOld = "",
                PasswordNew = request.PasswordNew
            };

            var expectedRes = 0;

            //Act
            var res = await _userService.ChangePassword(changePasswordModel);

            //Assert
            Assert.Equal(expectedRes, res);
        }

        [Fact]
        public async Task ChangePassword_Return0_NewPasswordIsEmpty()
        {
            // Arrage
            var request = GetChangePasswordRequest();
            var changePasswordModel = new ChangePasswordRequest()
            {
                UserId = request.UserId,
                PasswordOld = request.PasswordNew,
                PasswordNew = ""
            };

            var expectedRes = 0;

            //Act
            var res = await _userService.ChangePassword(changePasswordModel);

            //Assert
            Assert.Equal(expectedRes, res);
        }

        [Fact]
        public async Task ChangePassword_Return2_IncorrectPassword()
        {
            // Arrage
            var request = GetChangePasswordRequest();
            var oldPassword = "DifferentPasswordInDB";
            //PasswordHasher<User> hash = new PasswordHasher<User>();
            var changePasswordModel = new ChangePasswordRequest()
            {
                UserId = request.UserId,
                PasswordOld = oldPassword,
                PasswordNew = request.PasswordNew
            };
            var expectedRes = 2;

            //var _mock = new Mock<DbSet<User>>();
            //_mock.Setup(m => m.FindAsync(It.IsAny<object[]>())).Returns((object[] r) =>
            //{
            //    return new ValueTask<User>(userLogin);
            //});

            //Act
            var res = await _userService.ChangePassword(changePasswordModel);

            //Assert
            Assert.Equal(expectedRes, res);
        }

        [Fact]
        public async Task ChangePassword_Return1_ChangePasswordSuccess()
        {
            //Arrange
            var request = GetChangePasswordRequest();
            var passwordInDb = "PasswordInDB";
            var changePasswordModel = new ChangePasswordRequest()
            {
                UserId = request.UserId,
                PasswordOld = passwordInDb,
                PasswordNew = request.PasswordNew
            };
            var expectedRes = 1;

            //Act
            var res = await _userService.ChangePassword(changePasswordModel);

            //Assert
            Assert.Equal(expectedRes, res);
        }


        ////// Change Password FirstTime

        [Fact]
        public async Task ChangePasswordFirstTime_ReturnFalse_UseridIsNull()
        {
            // Arrage
            var request = GetChangePasswordRequestFirstTime();
            var changePasswordModel = new ChangePasswordFirstTimeModel()
            {
                UserId = "2D105F81-F259-4FD6-9DCE-5AB6E0EE6E3E",
                PasswordNew = request.PasswordNew
            };

            var expectedRes = false;

            //Act
            var res = await _userService.ChangePasswordFirstTime(changePasswordModel);

            //Assert
            Assert.Equal(expectedRes, res);
        }

        [Fact]
        public async Task ChangePasswordFirstTime_ReturnFalse_NewPasswordIsEmpty()
        {
            // Arrage
            var request = GetChangePasswordRequestFirstTime();
            var changePasswordModel = new ChangePasswordFirstTimeModel()
            {
                UserId = request.UserId,
                PasswordNew = ""
            };

            var expectedRes = false;

            //Act
            var res = await _userService.ChangePasswordFirstTime(changePasswordModel);

            //Assert
            Assert.Equal(expectedRes, res);
        }

        [Fact]
        public async Task ChangePasswordFirstTime_ReturnTrue_ChangePasswordFirstTimeSuccess()
        {
            //Arrange
            var request = new ChangePasswordFirstTimeModel
            {
                UserId = "AD6E8761-67C0-45AB-9EB6-0AE98EB888B1",
                PasswordNew = "NewPassword",
            };
            var changePasswordModel = new ChangePasswordFirstTimeModel()
            {
                UserId = request.UserId,
                PasswordNew = request.PasswordNew
            };
            var expectedRes = true;

            //Act
            var res = await _userService.ChangePasswordFirstTime(changePasswordModel);

            //Assert
            Assert.Equal(expectedRes, res);
        }

        [Fact]
        public async Task ChangePasswordFirstTime_ReturnFalse_NewUserIsFalse()
        {
            // Arrage
            var request = GetChangePasswordRequestFirstTime();
            var changePasswordModel = new ChangePasswordFirstTimeModel()
            {
                UserId = request.UserId,
                PasswordNew = "Quymlgnu"
            };

            var expectedRes = false;

            //Act
            var res = await _userService.ChangePasswordFirstTime(changePasswordModel);

            //Assert
            Assert.Equal(expectedRes, res);
        }
    }
}