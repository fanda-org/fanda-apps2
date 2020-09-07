using AutoMapper;
using Fanda.Auth.Controllers;
using Fanda.Service.AutoMapperProfile;
using FandaAuth.Domain;
using FandaAuth.Service;
using FandaAuth.Service.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FandaAuth.Tests
{
    //[PrioritizedFixture]
    [TestCaseOrderer("FandaAuth.Tests.PriorityOrderer", "FandaAuth.Tests")]
    public class ApplicationTests
    {
        private static readonly ApplicationRepository repository;
        public static DbContextOptions<AuthContext> DbContextOptions { get; }
        public static string ConnectionString = "Server=localhost;Database=fanda_auth_test;Port=3306;UID=tbala;PWD=tbm@123;";

        static ApplicationTests()
        {
            DbContextOptions = new DbContextOptionsBuilder<AuthContext>()
                .UseMySql(ConnectionString)
                .Options;

            // DbContext
            var context = new AuthContext(DbContextOptions);
            DbInitializer db = new DbInitializer();
            db.Seed(context);

            // AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AuthProfile());
            });
            var mapper = config.CreateMapper();

            repository = new ApplicationRepository(context, mapper);
        }

        public ApplicationTests()
        {
        }

        [Fact, TestPriority(1)]
        public async void Add_Ok()
        {
            //Arrange
            var controller = new ApplicationsController(repository)
            {
                // HttpContext
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            //Act
            var app = new ApplicationDto
            {
                Code = "FANDA",
                Name = "Fanda",
                Description = "Finance and accounting",
                Edition = "Standard",
                Version = "1.0.0",
                Active = true
            };
            var data = await controller.Create(app, string.Empty);

            //Assert
            Assert.IsType<CreatedAtActionResult>(data);
        }

        [Fact, TestPriority(2)]
        public async void GetAll_Ok()
        {
            //Arrange
            var controller = new ApplicationsController(repository)
            {
                // HttpContext
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            //Act
            var data = await controller.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact, TestPriority(3)]
        public void GetAll_Error()
        {
        }
    }
}
