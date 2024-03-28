using Application.Http;
using Application.Usuario.Interfaces;
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
using System.Net;
using System.Net.Http;
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
        private readonly IMapper _mapperMock;

        public UsuarioServicesTest(ITestOutputHelper output)
        {
            _output = output;

            _usuariosRepository = UsuariosRepositoryStub();
            var config = configIMapper();
            _mapperMock = config.CreateMapper();

            _usuariosService = new UsuarioServices(_usuariosRepository, _mapperMock);
        }

        private UsuariosRepository UsuariosRepositoryStub()
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            var dbContext = new ApplicationDbContext(options);
            return new UsuariosRepository(dbContext);
        }

        private MapperConfiguration configIMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UsuariosViewModel, Usuarios>()
                    .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
                    .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                    .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.password));

                cfg.CreateMap<Usuarios, UsuariosViewModel>()
                    .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
                    .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
                    .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email));
            });

            return config;
        }

        private void RemoverAllUsers()
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var usuariosParaRemover = dbContext.Usuarios.Where(u => u.id != 1).ToList();

                var usuario = new Usuarios
                {
                    id = 1,
                    name = "Fernando",
                    email = "fernando@gmail.com",
                    password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
                };

                if (usuariosParaRemover.Any())
                {
                    dbContext.Usuarios.RemoveRange(usuariosParaRemover);
                    dbContext.Usuarios.Update(usuario);
                    dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Usuarios', RESEED, 1)");
                    dbContext.SaveChanges();
                }
            }
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

            HttpResponse httpResponse = new HttpResponse();
            var usuariosService = new Mock<IUsuarioServices>();

            usuariosService.Setup(x => x.Add(usuariosViewModel)).Returns(httpResponse.Response(HttpStatusCode.InternalServerError, null, string.Empty));

            var result = usuariosService.Object.Add(usuariosViewModel);

            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Fact]
        public void UsuarioServices_ShouldReturnOkIfIdAllOk()
        {
            _output.WriteLine("Should return ok if is all ok");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            HttpResponse httpResponse = new HttpResponse();
            var usuariosService = new Mock<IUsuarioServices>();

            usuariosService.Setup(x => x.Add(usuariosViewModel)).Returns(httpResponse.Response(HttpStatusCode.OK, null, "OK"));

            HttpResponseMessage result = usuariosService.Object.Add(usuariosViewModel);
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async void UsuarioServices_ShouldCallGetByIdAndReturnAUser()
        {
            _output.WriteLine("Should call GetById and return a user");

            var result = await _usuariosService.GetById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.id);
            Assert.Equal("Fernando", result.name);
            Assert.Equal("fernando@gmail.com", result.email);
        }

        [Fact]
        public async void UsuarioServices_ShouldCallGetListdAndReturnAUserList()
        {
            _output.WriteLine("Should call GetList and return a users list");

            var lstUsuario = new List<Usuarios>
            {
                new Usuarios()
                {
                    name = "fernando1",
                    email = "fer@gmail.com",
                    password = "123456"
                },
                new Usuarios()
                {
                    name = "fernando2",
                    email = "fer2@gmail.com",
                    password = "123456",
                },
                new Usuarios()
                {
                    name = "fernando3",
                    email = "fer3@gmail.com",
                    password = "123456",
                },
            };

            foreach (var usuarioTeste in lstUsuario)
            {
                _usuariosRepository.Add(usuarioTeste);
            }

            var result = await _usuariosService.GetList();
            Assert.NotNull(result);
            Assert.True(result.Count() > 2);

            RemoverAllUsers();
        }

        [Fact]
        public async void UsuarioServices_ShouldCallUpdate()
        {
           _output.WriteLine("Should call Update and update user");

            var usuario = await _usuariosService.GetById(1);

            usuario.name = "santana";

            var statusReturn = _usuariosService.Update(usuario);

            var result = await _usuariosService.GetById(usuario.id);

            Assert.NotNull(result);
            Assert.Equal("santana", result.name);
            Assert.Equal(System.Net.HttpStatusCode.OK, statusReturn.StatusCode);

            result.name = "Fernando";
            _usuariosService.Update(result);

        }

        [Fact]
        public async void UsuarioServices_ShouldUpdateReturnBadRequest()
        {
            _output.WriteLine("Should return badRequest id Update not found user");

            var usuario = await _usuariosService.GetById(1);

            usuario.name = "santana";
            usuario.id = 1000;

            var statusReturn = _usuariosService.Update(usuario);

            Assert.NotNull(statusReturn);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, statusReturn.StatusCode);
            Assert.Equal("User not found!", statusReturn.ReasonPhrase);
        }

        [Fact]
        public async void UsuarioServices_ShouldCallAndRemoveUser()
        {
            _output.WriteLine("Should Call and Remove user");

            var usuario = new UsuariosViewModel()
            {
                name = "santanaFinal",
                email = "santana@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            _usuariosService.Add(usuario);

            var usuarioAdicionado = await _usuariosRepository.GetByEmail("santana@gmail.com");
            Assert.NotNull(usuarioAdicionado);

            var usuarioParaRemover = _mapperMock.Map<UsuariosViewModel>(usuarioAdicionado);
            var result = _usuariosService.Remove(usuarioParaRemover);

            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async void UsuarioServices_ShouldRemoveReturnBadRequest()
        {
            _output.WriteLine("Should return badRequest id Update not found user");

            var usuario = await _usuariosService.GetById(1);

            usuario.name = "santana";
            usuario.id = 1000;

            var statusReturn = _usuariosService.Remove(usuario);

            Assert.NotNull(statusReturn);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, statusReturn.StatusCode);
            Assert.Equal("User not found!", statusReturn.ReasonPhrase);
        }
    }
}
 