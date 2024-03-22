using Application.Login.Interfaces;
using Application.Login.Services;
using Application.Login.ViewModel;
using Application.Mapping;
using AutoMapper;
using Domain.Entities;
using Domain.Login.Entities;
using Infra.Data.Context;
using Infra.Data.Repository.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation;
using System.Linq;
using System.Threading.Tasks;
using Tests.Helper;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Login
{
    public class LoginControllerTest
    {
        private readonly ITestOutputHelper _output;
        private readonly LoginRepository _loginRepository;
        private readonly LoginServices _loginServices;
        private readonly Presentation.Controllers.Login _loginController;
        public LoginControllerTest(ITestOutputHelper output)
        {
            _output = output;
            _loginRepository = LoginRepositoryStub();
            var config = configIMapper();
            var mapperMock = config.CreateMapper();

            _loginServices = new LoginServices(_loginRepository, mapperMock);
            _loginController = new Presentation.Controllers.Login(_loginServices);
        }

        private LoginRepository LoginRepositoryStub()
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            var dbContext = new ApplicationDbContext(options);
            var configurationMock = appSettingsMock.configurationMockStub();

            return new LoginRepository(dbContext, configurationMock.Object);
        }

        private MapperConfiguration configIMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<LoginEntryViewModel, LoginEntry>();
                cfg.CreateMap<LoginEntry, LoginEntryViewModel>();
            });

            return config;
        }

        [Fact]
        public async Task LoginRepository_ShouldCallLoginController()
        {
            _output.WriteLine("Should call LoginController");

            var loginEntry = new LoginEntryViewModel()
            {
                email = "fer@gmail.com",
                password = "123456"
            };

            var result = await _loginController.LoginEntry(loginEntry);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task LoginRepository_ShouldReturnStatus403Forbidden()
        {
            _output.WriteLine("Should return the status StatusCodes.Status403Forbidden");

            var loginEntry = new LoginEntryViewModel()
            {
                email = "fer@gmail.com",
                password = "123456"
            };

            var result = await _loginController.LoginEntry(loginEntry);
                
            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
            }
        }

        [Fact]
        public async Task LoginRepository_ShouldReturnStatus400BadRequestEmailMissing()
        {
            _output.WriteLine("Should return the status Status400BadRequest if e-mail is missing.");

            var loginEntry = new LoginEntryViewModel()
            {
                email = "",
                password = "123456"
            };

            var result = await _loginController.LoginEntry(loginEntry);

            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            }
        }

        [Fact]
        public async Task LoginRepository_ShouldReturnStatus400BadRequestPasswordMissing()
        {
            _output.WriteLine("Should return the status Status400BadRequest if password is missing.");

            var loginEntry = new LoginEntryViewModel()
            {
                email = "fer@gmail.com",
                password = ""
            };

            var result = await _loginController.LoginEntry(loginEntry);

            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            }
        }

        [Fact]
        public async Task LoginRepository_ShouldReturnStatus200IfOK()
        {
            _output.WriteLine("Should return the status 200OK if ok.");

            var loginEntry = new LoginEntryViewModel()
            {
                email = "fernando@gmail.com",
                password = "123456"
            };

            var result = await _loginController.LoginEntry(loginEntry);

            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            }
        }
    }
}
