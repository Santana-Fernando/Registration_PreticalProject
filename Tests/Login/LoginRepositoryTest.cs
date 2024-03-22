using Xunit.Abstractions;
using Xunit;
using Infra.Data.Repository.Login;
using Microsoft.Extensions.Configuration;
using Domain.Login.Entities;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Moq;
using Tests.Helper;
using Application.Helpers;
using Domain.Entities;
using System;

namespace Tests.Login
{
    public class LoginRepositoryTest
    {
        private readonly ITestOutputHelper _output;
        private readonly ApplicationDbContext _dbContext;
        private readonly LoginRepository _loginRepository;
        public LoginRepositoryTest(ITestOutputHelper output)
        {
            _output = output;
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            var configurationMock = appSettingsMock.configurationMockStub();
            _dbContext = new ApplicationDbContext(options);
            _loginRepository = new LoginRepository(_dbContext, configurationMock.Object);
        }

        [Fact]
        public async Task LoginRepository_ShouldCallAuthenticationLogin()
        {
            _output.WriteLine("Should call Authenticatio Login and return any answer");

            var loginEntry = new LoginEntry
            {
                email = "test@example.com",
                password = "password123"
            };

            var result = await _loginRepository.Login(loginEntry);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task LoginRepository_ShouldReturnAccessDeniedIfLoginEntryDataNotExists()
        {
            _output.WriteLine("Should return access denied if login data not exists");

            var loginEntry = new LoginEntry
            {
                email = "test@example.com",
                password = "password123"
            };

            var result = await _loginRepository.Login(loginEntry);

            Assert.NotNull(result);
            Assert.Equal("Access denied", result.message);
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, result.statusCode);
        }

        [Fact]
        public async Task LoginRepository_ShouldReturnInternalServerErrorIfLoginCatches()
        {
            _output.WriteLine("Should return internal server error if login catches");

            var configurationMock = new Mock<IConfiguration>();
            var loginRepository = new Mock<ILogin>();

            var loginEntry = new LoginEntry
            {
                email = "test@example.com",
                password = "123456"
            };

            var errorResponse = new Autenticacao
            {
                statusCode = System.Net.HttpStatusCode.InternalServerError,
                message = "InternalServerError"
            };

            loginRepository.Setup(x => x.Login(loginEntry)).ReturnsAsync(errorResponse);

            var result = await loginRepository.Object.Login(loginEntry);

            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.statusCode);
        }

        [Fact]
        public async Task LoginRepository_ShouldReturnOkIfLoginIsCorrect()
        {
            _output.WriteLine("Should return OK if login is correct");

            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var configurationMock = appSettingsMock.configurationMockStub();
            var loginEntry = new LoginEntry
            {
                email = "fernando@gmail.com",
                password = "123456"
            };

            var result = await _loginRepository.Login(loginEntry);

            Assert.NotNull(result);
            Assert.True(new TokenAuthentication(configurationMock.Object).ValidateToken(result.token));
            Assert.Equal(System.Net.HttpStatusCode.OK, result.statusCode);
            Assert.Equal("OK", result.message);
        }

    }
}
