using Application.Usuario.Services;
using Application.Usuario.ViewModel;
using Application.Validation;
using AutoMapper;
using Domain.Usuario.Entidades;
using Domain.Usuario.Interfaces;
using Infra.Data.Context;
using Infra.Data.Repository.Usuario;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tests.Helper;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Usuario
{
    public class UsuarioServicesTest
    {
        private readonly ITestOutputHelper _output;
        private readonly UsuarioServices _usuariosService;
        private readonly UsuariosRepository _usuariosRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<IMapper> _mapperMock;

        public UsuarioServicesTest(ITestOutputHelper output)
        {
            _output = output;

            _usuariosRepository = UsuariosRepositoryStub();
            _mapperMock = new Mock<IMapper>();

            _usuariosService = new UsuarioServices(_usuariosRepository, _mapperMock.Object);
        }

        private UsuariosRepository UsuariosRepositoryStub()
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            var dbContext = new ApplicationDbContext(options);
            return new UsuariosRepository(dbContext);
        }

        #region ValidateFields
        [Fact]
        public void UsuarioServices_ShouldValidateFieldNameIsEmpty()
        {
            _output.WriteLine("Should validate field Name is empty");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = string.Empty,
                email = "fer@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The Name is Required");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFieldNameIsLong()
        {
            _output.WriteLine("Should validate field Name is Long");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "asdfasdfasfasfsadfjasdfjasdjfjasdklfjlasdkjfklçasjfljasdlçkfjlkasdjfklasdjfajsdfjasdkfjasdkjfçjfkjsadjd",
                email = "fer@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field name must be a string or array type with a maximum length of '100'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFieldNameIsShort()
        {
            _output.WriteLine("Should validate field Name is short");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "2D",
                email = "fer@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field name must be a string or array type with a minimum length of '3'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFieldEmailIsEmpty()
        {
            _output.WriteLine("Should validate field Email is empty");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = string.Empty,
                password = "123456",
                passwordConfirmation = "123456"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The E-mail is Required");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFieldEmailIsLong()
        {
            _output.WriteLine("Should validate field Email is Long");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "asdfasdfasfasfsadfjasdfjasdjfjasdklfjlasdkjfklçasjfljasdlçkfjlkasdjfklasdjfajsdfjasdkfjasdkjfçjfkjsadjd@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field email must be a string or array type with a maximum length of '100'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFieldEmailIsShort()
        {
            _output.WriteLine("Should validate field Email is Short");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "f@l",
                password = "123456",
                passwordConfirmation = "123456"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field email must be a string or array type with a minimum length of '5'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFieldPasswordIsEmpty()
        {
            _output.WriteLine("Should validate field Password is empty");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = string.Empty,
                passwordConfirmation = "123456"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The Password is Required");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFieldPasswordIsLong()
        {
            _output.WriteLine("Should validate field Password is Long");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "123412341234",
                passwordConfirmation = "123456"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field password must be a string or array type with a maximum length of '10'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFieldPasswordIsShort()
        {
            _output.WriteLine("Should validate field Password is Short");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "123",
                passwordConfirmation = "123456"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field password must be a string or array type with a minimum length of '6'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFieldPasswordConfirmationIsEmpty()
        {
            _output.WriteLine("Should validate field Password Confirmation is empty");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "123456",
                passwordConfirmation = string.Empty
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The Password need to be confirmed");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFieldPasswordConfirmationIsNotEqualToPassword()
        {
            _output.WriteLine("Should validate field Password Confirmation is not equal Password");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "123456",
                passwordConfirmation = "1234567"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The password and confirmation password do not match.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact]
        public void UsuarioServices_ShouldValidateFielsdAndReturOk()
        {
            _output.WriteLine("Should validate fields and return ok");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            var validationFields = new ValidationFields(usuariosViewModel).IsValidWithErrors();

            Assert.True(validationFields.IsValid);
        }
        #endregion

        #region ValidateFields
        [Fact]
        public void UsuarioServices_ShouldCallTheFunctionAdd()
        {
            _output.WriteLine("Should call the function add");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "",
                email = "",
                password = "",
                passwordConfirmation = ""
            };

            var result = _usuariosService.Add(usuariosViewModel);

            Assert.NotNull(result);
        }

        [Fact]
        public void UsuarioServices_ShouldReturnBadRequestIfFieldsIsInvalid()
        {
            _output.WriteLine("Should return badRequest if fields is invalid");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "",
                email = "",
                password = "",
                passwordConfirmation = ""
            };

            var result = _usuariosService.Add(usuariosViewModel);

            Assert.NotNull(result);
            Assert.Equal("Some fields are with invalid values.", result.ReasonPhrase);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

        }

        [Fact]
        public void UsuarioServices_ShouldReturnBadRequestIfEmailIsRegistered()
        {
            _output.WriteLine("Should return badRequest if email is registered");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fernando@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            var result = _usuariosService.Add(usuariosViewModel);

            Assert.NotNull(result);
            Assert.Equal("There is already a registered user with the email: fernando@gmail.com.", result.ReasonPhrase);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

        }
        [Fact]
        public void UsuarioServices_ShouldReturnInternalServerErrorIfProcessFalied()
        {
            _output.WriteLine("Should return a internal server erro if process falied");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            var mappedUsuarios = new Usuarios
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
            };

            _mapperMock.Setup(x => x.Map<UsuariosViewModel>(usuariosViewModel)).Returns(usuariosViewModel);

            var result = _usuariosService.Add(usuariosViewModel);

            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);

        }

        [Fact]
        public async void UsuarioServices_ShouldReturnOkIfIdAllOk()
        {
            _output.WriteLine("Should return ok if is all ok");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            var mappedUsuarios = new Usuarios
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
            };

            _mapperMock.Setup(x => x.Map<Usuarios>(usuariosViewModel)).Returns(mappedUsuarios);

            var result = _usuariosService.Add(usuariosViewModel);
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

            var usuarioRemovido = await _usuariosRepository.GetByEmail("fer@gmail.com");
            _usuariosRepository.Remove(usuarioRemovido);
        }
        #endregion
    }
}
 