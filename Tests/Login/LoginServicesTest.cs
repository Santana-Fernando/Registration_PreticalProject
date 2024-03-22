using Application.Login.Services;
using Application.Login.ViewModel;
using Application.Validation;
using AutoMapper;
using Domain.Entities;
using Domain.Login.Entities;
using Infra.Data.Context;
using Infra.Data.Repository.Login;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tests.Helper;
using Xunit;
using Xunit.Abstractions;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Tests.Login
{
    public class LoginServicesTest
    {
        private readonly ITestOutputHelper _output;
        private readonly LoginServices _loginServices;
        private readonly IMapper _mapperMock;
        public LoginServicesTest(ITestOutputHelper output)
        {
            _output = output;
            _loginServices = LoginRepositoryStub();
            var config = configIMapper();
            _mapperMock = config.CreateMapper();
        }

        private LoginServices LoginRepositoryStub()
        {
            var loginRepositoryMock = new Mock<ILogin>();
            return new LoginServices(loginRepositoryMock.Object, _mapperMock);
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
        public async Task LoginServices_ShouldCallFunctionLogin()
        {
            _output.WriteLine("Should call function login");

            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "test@example.com",
                password = "password123"
            };

            var result = await _loginServices.Login(loginEntryViewModel);
            Assert.NotNull(result);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessageEmailRequired()
        {
            _output.WriteLine("Should return the message of email required");
            
            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "",
                password = "123456"
            };

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The E-mail is Required");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessageEmailMinCharacter()
        {
            _output.WriteLine("Should return the message min Email characteres numbers");
            
            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "f@m",
                password = "123456"
            };

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The field email must be a string or array type with a minimum length of '10'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessageEmailMaxCharacter()
        {
            _output.WriteLine("Should return the message Max Email characteres numbers");
            
            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "asdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdf@gmail.com",
                password = "123456"
            };

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The field email must be a string or array type with a maximum length of '100'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessagePasswordlRequired()
        {
            _output.WriteLine("Should return the message of password required");
            
            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "fer@gmail.com",
                password = ""
            };

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The Password is Required");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessagePasswordMinCharacter()
        {
            _output.WriteLine("Should return the message min password characteres numbers");
            
            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "fer@gmail.com",
                password = "123"
            };

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The field password must be a string or array type with a minimum length of '6'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessagePasswordMaxCharacter()
        {
            _output.WriteLine("Should return the message Max Password characteres numbers");
            
            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "fer@gmail.com",
                password = "1234123412341234"
            };

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The field password must be a string or array type with a maximum length of '10'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnTheFieldsAreOkAndValid()
        {
            _output.WriteLine("Should return that the fields are ok and valid");
            
            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "fer@gmail.com",
                password = "123456"
            };

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            
            Assert.True(validationFields.IsValid);
        }
    }
}
