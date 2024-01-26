using Application.Login.Services;
using Application.Login.ViewModel;
using Application.Validation;
using AutoMapper;
using Domain.Entities;
using Domain.Login.Entities;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Tests.Login
{
    public class LoginServicesTest
    {
        private readonly ITestOutputHelper _output;
        public LoginServicesTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task LoginServices_ShouldCallFunctionLogin()
        {
            _output.WriteLine("Should call function login");

            var loginRepositoryMock = new Mock<ILogin>();
            var mapperMock = new Mock<IMapper>();
            var loginServices = new LoginServices(loginRepositoryMock.Object, mapperMock.Object);

            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "test@example.com",
                password = "password123"
            };

            var mappedLoginEntry = new LoginEntry
            {
                email = "test@example.com",
                password = "password123"
            };

            mapperMock.Setup(x => x.Map<LoginEntry>(loginEntryViewModel)).Returns(mappedLoginEntry);

            var result = await loginServices.Login(loginEntryViewModel);
            Assert.NotNull(result);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessageEmailRequired()
        {
            _output.WriteLine("Should return the message of email required");
            // Arrange
            var loginRepositoryMock = new Mock<ILogin>();
            var mapperMock = new Mock<IMapper>();
            var loginServices = new LoginServices(loginRepositoryMock.Object, mapperMock.Object);

            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "",
                password = "123456"
            };

            var mappedLoginEntry = new LoginEntry
            {
                email = "fer@gmail.com",
                password = "123456"
            };

            mapperMock.Setup(x => x.Map<LoginEntry>(loginEntryViewModel)).Returns(mappedLoginEntry);

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The E-mail is Required");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessageEmailMinCharacter()
        {
            _output.WriteLine("Should return the message min Email characteres numbers");
            // Arrange
            var loginRepositoryMock = new Mock<ILogin>();
            var mapperMock = new Mock<IMapper>();
            var loginServices = new LoginServices(loginRepositoryMock.Object, mapperMock.Object);

            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "f@m",
                password = "123456"
            };

            var mappedLoginEntry = new LoginEntry
            {
                email = "f@m",
                password = "123456"
            };

            mapperMock.Setup(x => x.Map<LoginEntry>(loginEntryViewModel)).Returns(mappedLoginEntry);

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The field email must be a string or array type with a minimum length of '10'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessageEmailMaxCharacter()
        {
            _output.WriteLine("Should return the message Max Email characteres numbers");
            // Arrange
            var loginRepositoryMock = new Mock<ILogin>();
            var mapperMock = new Mock<IMapper>();
            var loginServices = new LoginServices(loginRepositoryMock.Object, mapperMock.Object);

            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "asdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdf@gmail.com",
                password = "123456"
            };

            var mappedLoginEntry = new LoginEntry
            {
                email = "asdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdf@gmail.com",
                password = "123456"
            };

            mapperMock.Setup(x => x.Map<LoginEntry>(loginEntryViewModel)).Returns(mappedLoginEntry);

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The field email must be a string or array type with a maximum length of '100'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessagePasswordlRequired()
        {
            _output.WriteLine("Should return the message of password required");
            // Arrange
            var loginRepositoryMock = new Mock<ILogin>();
            var mapperMock = new Mock<IMapper>();
            var loginServices = new LoginServices(loginRepositoryMock.Object, mapperMock.Object);

            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "fer@gmail.com",
                password = ""
            };

            var mappedLoginEntry = new LoginEntry
            {
                email = "fer@gmail.com",
                password = "123456"
            };

            mapperMock.Setup(x => x.Map<LoginEntry>(loginEntryViewModel)).Returns(mappedLoginEntry);

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The Password is Required");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessagePasswordMinCharacter()
        {
            _output.WriteLine("Should return the message min password characteres numbers");
            // Arrange
            var loginRepositoryMock = new Mock<ILogin>();
            var mapperMock = new Mock<IMapper>();
            var loginServices = new LoginServices(loginRepositoryMock.Object, mapperMock.Object);

            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "fer@gmail.com",
                password = "123"
            };

            var mappedLoginEntry = new LoginEntry
            {
                email = "fer@gmail.com",
                password = "123"
            };

            mapperMock.Setup(x => x.Map<LoginEntry>(loginEntryViewModel)).Returns(mappedLoginEntry);

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The field password must be a string or array type with a minimum length of '6'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnMessagePasswordMaxCharacter()
        {
            _output.WriteLine("Should return the message Max Password characteres numbers");
            // Arrange
            var loginRepositoryMock = new Mock<ILogin>();
            var mapperMock = new Mock<IMapper>();
            var loginServices = new LoginServices(loginRepositoryMock.Object, mapperMock.Object);

            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "fer@gmail.com",
                password = "1234123412341234"
            };

            var mappedLoginEntry = new LoginEntry
            {
                email = "fer@gmail.com",
                password = "1234123412341234"
            };

            mapperMock.Setup(x => x.Map<LoginEntry>(loginEntryViewModel)).Returns(mappedLoginEntry);

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The field password must be a string or array type with a maximum length of '10'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void LoginServices_ShouldReturnTheFieldsAreOkAndValid()
        {
            _output.WriteLine("Should return that the fields are ok and valid");
            // Arrange
            var loginRepositoryMock = new Mock<ILogin>();
            var mapperMock = new Mock<IMapper>();
            var loginServices = new LoginServices(loginRepositoryMock.Object, mapperMock.Object);

            var loginEntryViewModel = new LoginEntryViewModel
            {
                email = "fer@gmail.com",
                password = "123456"
            };

            var mappedLoginEntry = new LoginEntry
            {
                email = "fer@gmail.com",
                password = "123456"
            };

            mapperMock.Setup(x => x.Map<LoginEntry>(loginEntryViewModel)).Returns(mappedLoginEntry);

            var validationFields = new ValidationFields(loginEntryViewModel).IsValidWithErrors();
            
            Assert.True(validationFields.IsValid);
        }
    }
}
