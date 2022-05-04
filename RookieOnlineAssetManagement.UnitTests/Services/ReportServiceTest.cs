using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Enums;
using RookieOnlineAssetManagement.Models.Common;
using RookieOnlineAssetManagement.Models.Reports;
using RookieOnlineAssetManagement.Services;
using Xunit;

namespace RookieOnlineAssetManagement.UnitTests.Services
{
	public class ReportServiceTest
	{
		private readonly ApplicationDbContext _applicationDbContext;
		private readonly ReportService _reportService;
        private readonly Mock<UserManager<User>> _mockUserManager;

        //Setup test
        public ReportServiceTest()
        {
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            using(var context = new ApplicationDbContext(options))
            {
                context.Categories.AddRange(
                   new Category
                   {
                       Id = 1,
                       CategoryName = "Personal Computer",
                       Prefix = "PC",
                   },
                   new Category
                   {
                       Id = 2,
                       CategoryName = "Laptop",
                       Prefix = "LA",
                   }
                );

                context.Assets.AddRange(
                    new Asset
                    {
                        Id = 1,
                        AssetCode = "PC000001",
                        AssetName = "Mac Mini M1",
                        Specification = "Lorem ipsum ",
                        InstalledDate = DateTime.Now,
                        AssetState = AssetState.Assigned,
                        CategoryId = 1,
                        LocationId = "hn",
                        Status = Status.Active,
                        ModifiedDate = DateTime.Now
                    },
                    new Asset
                    {
                        Id = 2,
                        AssetCode = "LA000001",
                        AssetName = "Laptop Gaming VNG26 i5 9400F",
                        Specification = "Lorem ipsum ",
                        InstalledDate = DateTime.Now,
                        AssetState = AssetState.Available,
                        CategoryId = 2,
                        LocationId = "hcm",
                        Status = Status.Active,
                        ModifiedDate = DateTime.Now
                    },
                    new Asset
                    {
                        Id = 3,
                        AssetCode = "LA000002",
                        AssetName = "Macbook M1 Pro",
                        Specification = "Lorem ipsum ",
                        InstalledDate = DateTime.Now,
                        AssetState = AssetState.WaitingForRecycling,
                        CategoryId = 2,
                        LocationId = "hn",
                        Status = Status.Active,
                        ModifiedDate = DateTime.Now
                    },
                    new Asset
                    {
                        Id = 4,
                        AssetCode = "LA000003",
                        AssetName = "Macbook M2 Pro",
                        Specification = "Lorem ipsum ",
                        InstalledDate = DateTime.Now,
                        AssetState = AssetState.Assigned,
                        CategoryId = 2,
                        LocationId = "hn",
                        Status = Status.Active,
                        ModifiedDate = DateTime.Now
                    },
                    new Asset
                    {
                        Id = 5,
                        AssetCode = "LA000004",
                        AssetName = "Macbook M3 Pro",
                        Specification = "Lorem ipsum ",
                        InstalledDate = DateTime.Now,
                        AssetState = AssetState.Available,
                        CategoryId = 2,
                        LocationId = "hn",
                        Status = Status.Active,
                        ModifiedDate = DateTime.Now
                    }
                 );
                context.SaveChanges();
            }

            var mockContext = new ApplicationDbContext(options);
            _reportService = new ReportService(mockContext, _mockUserManager.Object);
        }

        private GetReportPagingRequest GetPagingRequest()
        { 
            return new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "",
                OrderMode = Enums.OrderMode.ASC
            };
        }

        private User GetFakeUser()
        {
            return new User()
            {
                Id = "5ed46d2e-cbfe-4ea9-80fe-5bf772c2cdef",
                FirstName = "Huy",
                LastName = "Nguyen",
                Email = "huy312100@gmail.com",
                BirthDay = new DateTime(2000, 10, 31),
                EmailConfirmed = true,
                NormalizedEmail = "huy312100@gmail.com",
                UserName = "huy312100",
                NormalizedUserName = "huy312100",
                LocationId = "hn",
                Gender = Enums.Gender.Male,
                JoinedDate = DateTime.Now,
                IsNew = false
            };
        }

        private List<ReportModel> Result()
        {
            var data = new List<ReportModel>()
            {
                new ReportModel()
                {
                    Category = "Personal Computer",
                    Total = 1,
                    Assigned = 1,
                    Available = 0,
                    NotAvailable = 0,
                    WaitingForRecycling = 0,
                    Recycled = 0,
                },
                new ReportModel()
                {
                    Category = "Laptop",
                    Total = 3,
                    Assigned = 1,
                    Available = 1,
                    NotAvailable = 0,
                    WaitingForRecycling = 1,
                    Recycled = 0,
                }
            };
            return data;
        }

        private List<ReportModel> ResultSort()
        {
            var data = new List<ReportModel>()
            {
                new ReportModel()
                {
                    Category = "Laptop",
                    Total = 3,
                    Assigned = 1,
                    Available = 1,
                    NotAvailable = 0,
                    WaitingForRecycling = 1,
                    Recycled = 0,
                },
                new ReportModel()
                {
                    Category = "Personal Computer",
                    Total = 1,
                    Assigned = 1,
                    Available = 0,
                    NotAvailable = 0,
                    WaitingForRecycling = 0,
                    Recycled = 0,
                }
            };
            return data;
        }

        // Test function
        [Fact]
        public async Task GetAllPaging_ReturnItemNull_InValidAdminId()
        {
            // Arrange
            var request = GetPagingRequest();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = 0,
                Items = null,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());

            //Act
            var result = await _reportService.GetAllPaging(It.IsAny<string>(), request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            Assert.Equal(resultExpected.Items, result.Items);
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultReportViewModel_ValidAdminId()
        {
            // Arrange
            var request = GetPagingRequest();

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        //Unit test for column sort
        //Category sort
        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithCategoryAsc_OrderWithCategoryColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "category",
                OrderMode = Enums.OrderMode.ASC
            };

            var dataReport = ResultSort();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithCategoryDesc_OrderWithCategoryColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "category",
                OrderMode = Enums.OrderMode.DES
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        //Total sort
        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithTotalAsc_OrderWithTotalColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "total",
                OrderMode = Enums.OrderMode.ASC
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithTotalDesc_OrderWithTotalColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "total",
                OrderMode = Enums.OrderMode.DES
            };

            var dataReport = ResultSort();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        //Assigned sort
        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithAssignedAsc_OrderWithAssignedColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "assigned",
                OrderMode = Enums.OrderMode.ASC
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithAssignedDesc_OrderWithAssignedColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "assigned",
                OrderMode = Enums.OrderMode.DES
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        //Available sort
        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithAvailableAsc_OrderWithAvailableColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "available",
                OrderMode = Enums.OrderMode.ASC
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithAvailableDesc_OrderWithAvailableColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "available",
                OrderMode = Enums.OrderMode.DES
            };

            var dataReport = ResultSort();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        //Not available sort
        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithNotAvailableAsc_OrderWithNotAvailableColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "notAvailable",
                OrderMode = Enums.OrderMode.ASC
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithNotAvailableDesc_OrderWithNotAvailableColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "notAvailable",
                OrderMode = Enums.OrderMode.DES
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        //Wating for recycling sort 
        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithWaitingForRecyclingAsc_OrderWithWaitingForRecyclingColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "waitingForRecycling",
                OrderMode = Enums.OrderMode.ASC
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithWaitingForRecyclingDesc_OrderWithWaitingForRecyclingColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "waitingForRecycling",
                OrderMode = Enums.OrderMode.DES
            };

            var dataReport = ResultSort();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        //Recycled sort
        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithRecycledAsc_OrderWithRecycledColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "recycled",
                OrderMode = Enums.OrderMode.ASC
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        [Fact]
        public async Task GetAllPaging_ReturnPagedResultWithRecycledDesc_OrderWithRecycledColumn()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = false,
                PageIndex = 1,
                PageSize = 10,
                OrderName = "recycled",
                OrderMode = Enums.OrderMode.DES
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }

        //Unit test for get all data is true
        [Fact]
        public async Task GetAllPaging_ReturnPagedResultReportViewModel_GetAllIsTrue()
        {
            // Arrange
            var request = new GetReportPagingRequest
            {
                GetAll = true,
                PageIndex = 0,
                PageSize = 0,
                OrderName = "",
                OrderMode = Enums.OrderMode.ASC
            };

            var dataReport = Result();

            var resultExpected = new PagedResult<ReportModel>
            {
                TotalRecord = dataReport.Count,
                Items = dataReport,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var adminAccount = GetFakeUser();

            _mockUserManager.Setup(um => um.FindByIdAsync(adminAccount.Id)).ReturnsAsync(adminAccount);

            //Act
            var result = await _reportService.GetAllPaging(adminAccount.Id, request);

            //Assert
            Assert.Equal(resultExpected.TotalRecord, result.TotalRecord);
            Assert.Equal(resultExpected.PageIndex, result.PageIndex);
            Assert.Equal(resultExpected.PageSize, result.PageSize);
            Assert.Equal(Int32.MinValue, result.PageCount);
            for (var i = 0; i < result.TotalRecord; i++)
            {
                Assert.Equal(resultExpected.Items[i].Category, result.Items[i].Category);
                Assert.Equal(resultExpected.Items[i].Total, result.Items[i].Total);
                Assert.Equal(resultExpected.Items[i].Available, result.Items[i].Available);
                Assert.Equal(resultExpected.Items[i].NotAvailable, result.Items[i].NotAvailable);
                Assert.Equal(resultExpected.Items[i].Assigned, result.Items[i].Assigned);
                Assert.Equal(resultExpected.Items[i].WaitingForRecycling, result.Items[i].WaitingForRecycling);
                Assert.Equal(resultExpected.Items[i].Recycled, result.Items[i].Recycled);
            }
        }
    }
}

