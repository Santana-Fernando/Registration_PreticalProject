using Xunit.Abstractions;
using Xunit;
using Infra.Data.Repository.Usuario;
using Application.Usuario.Services;
using Tests.Helper;
using Infra.Data.Context;
using AutoMapper;
using Application.Usuario.ViewModel;
using Domain.Usuario.Entidades;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Application.Usuario.Interfaces;
using Moq;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Tests.Usuario
{
    public class UsuarioControllerTest
    {
        private readonly ITestOutputHelper _output;
        private readonly UsuarioServices _usuariosService;
        private readonly UsuariosRepository _usuariosRepository;
        private readonly Presentation.Controllers.Usuario _usuarioController;
        public UsuarioControllerTest(ITestOutputHelper output)
        {
            _output = output;
            _usuariosRepository = UsuariosRepositoryStub();
            var config = configIMapper();
            var mapperMock = config.CreateMapper();

            _usuariosService = new UsuarioServices(_usuariosRepository, mapperMock);
            _usuarioController = new Presentation.Controllers.Usuario(_usuariosService);
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

        [Fact]
        public async void UsuarioController_ShouldCallGetList()
        {
            _output.WriteLine("Should call GetList");

            var result = await _usuarioController.GetList();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UsuarioController_ShouldValidateTheAuthorizationTokenAndReturnUnauthorized()
        {
            _output.WriteLine("Should validate the authorization token and return Unauthorized");

            var usuarioServicesMock = new Mock<IUsuarioServices>();
            usuarioServicesMock.Setup(x => x.GetList()).ThrowsAsync(new Exception("Unauthorized"));
            var controller = new Presentation.Controllers.Usuario(usuarioServicesMock.Object);

            var result = await controller.GetList();

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UsuarioController_ShouldValidateTheAuthorizationTokenAndReturnOK() 
        {
            _output.WriteLine("Should validate the authorization token and return ok");
            
            var result = await _usuarioController.GetList();

            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public void UsuarioController_shouldCallRegister()
        {
            _output.WriteLine("Should call the register");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel();
            var result = _usuarioController.Register(usuariosViewModel);

            Assert.NotNull(result);
        }

        [Fact]
        public void UsuarioController_shouldReturn400()
        {
            _output.WriteLine("Should return 400 register return error");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel();
            var result = _usuarioController.Register(usuariosViewModel);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            }
        }

        [Fact]
        public void UsuarioController_shouldReturn401IfRegisterIsUnauthorized()
        {
            _output.WriteLine("Should return 401 if register is unauthorized");

            var usuarioServicesMock = new Mock<IUsuarioServices>();
            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "santana",
                email = "sant@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };
            usuarioServicesMock.Setup(x => x.Add(usuariosViewModel)).Throws(new Exception("Unauthorized"));
            var controller = new Presentation.Controllers.Usuario(usuarioServicesMock.Object);

            var result = controller.Register(usuariosViewModel);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Fact]
        public void UsuarioController_shouldReturn200IfOk()
        {
            RemoverAllUsers();
            _output.WriteLine("Should return 200 if Register OK");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel() { 
                name = "santana",
                email =  "sant@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };
            var result = _usuarioController.Register(usuariosViewModel);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            }
        }

        [Fact]
        public void UsuarioController_shouldReturn500AddIfNotOk()
        {
            _output.WriteLine("Should return 500 if Register not OK");

            Mock<IMapper> mapperMock = new Mock<IMapper>();
            var usuariosService = new UsuarioServices(_usuariosRepository, mapperMock.Object);

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "123456",
                passwordConfirmation = "123456"
            };

            var mappedUsuarios = new Usuarios()
            {
                name = "fernando",
                email = "fer@gmail.com",
                password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
            };

            mapperMock.Setup(x => x.Map<UsuariosViewModel>(usuariosViewModel)).Returns(usuariosViewModel);

            var controller = new Presentation.Controllers.Usuario(usuariosService);

            var result = controller.Register(usuariosViewModel);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            }
        }

        [Fact]
        public void UsuarioController_shouldCallFunctionGetById()
        {
            _output.WriteLine("Should call Function GetById");

            var result = _usuarioController.GetById(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UsuarioController_shouldReturn401IfGetByIdUnauthorized()
        {
            _output.WriteLine("Should return 401 if GetById Unauthorized");

            var usuarioServicesMock = new Mock<IUsuarioServices>();
            usuarioServicesMock.Setup(x => x.GetById(1)).ThrowsAsync(new Exception("Unauthorized"));
            var controller = new Presentation.Controllers.Usuario(usuarioServicesMock.Object);

            var result = await controller.GetById(1);
            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UsuarioController_shouldReturn200if()
        {
            _output.WriteLine("Should call Function GetById");

            var result = await _usuarioController.GetById(1);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            }
        }

        [Fact]
        public void UsuarioController_shouldCallFunctionUpdate()
        {
            _output.WriteLine("Should call Function Update");
            UsuariosViewModel usuariosViewModel = new UsuariosViewModel();
            var result = _usuarioController.Update(usuariosViewModel);

            Assert.NotNull(result);
        }

        [Fact]
        public void UsuarioController_shouldReturn401IfUpdateUnauthorized()
        {
            _output.WriteLine("Should return 401 Update Unauthorized");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel();
            var usuarioServicesMock = new Mock<IUsuarioServices>();
            usuarioServicesMock.Setup(x => x.Update(usuariosViewModel)).Throws(new Exception("Unauthorized"));
            var controller = new Presentation.Controllers.Usuario(usuarioServicesMock.Object);

            var result = controller.Update(usuariosViewModel);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Fact]
        public void UsuarioController_shouldReturn404NotFoundIfUpdateDontFindUser()
        {
            _output.WriteLine("Should return 404 if update dont found user");
            UsuariosViewModel usuariosViewModel = new UsuariosViewModel();
            var result = _usuarioController.Update(usuariosViewModel);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            }
        }

        [Fact]
        public void UsuarioController_shouldReturn200UpdateIsOk()
        {
            _output.WriteLine("Should return 200 if update is Ok");
            UsuariosViewModel usuariosViewModel = new UsuariosViewModel()
            {
                id = 1,
                name = "Fernando",
                email = "fernando@gmail.com"
            };

            var result = _usuarioController.Update(usuariosViewModel);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            }
        }

        [Fact]
        public void UsuarioController_shouldCallFunctionRemove()
        {
            _output.WriteLine("Should call function Remove");
            UsuariosViewModel usuariosViewModel = new UsuariosViewModel();

            var result = _usuarioController.Remove(usuariosViewModel);

            Assert.NotNull(result);
        }

        [Fact]
        public void UsuarioController_shouldReturn401IfRemoveIsUnauthorized()
        {
            _output.WriteLine("Should return 401 delete id Unauthorized");

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel();
            var usuarioServicesMock = new Mock<IUsuarioServices>();
            usuarioServicesMock.Setup(x => x.Remove(usuariosViewModel)).Throws(new Exception("Unauthorized"));
            var controller = new Presentation.Controllers.Usuario(usuarioServicesMock.Object);

            var result = controller.Remove(usuariosViewModel);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Fact]
        public void UsuarioController_shouldReturn404NotFoundIfRemoveDontFindUser()
        {
            _output.WriteLine("Should return 404 if remove dont found user");
            UsuariosViewModel usuariosViewModel = new UsuariosViewModel();

            var result = _usuarioController.Remove(usuariosViewModel);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            }
        }

        [Fact]
        public void UsuarioController_shouldReturn200IfRemoveIsOK()
        {
            _output.WriteLine("Should return 200 delete id is ok");
            Application.Http.HttpResponse httpResponse = new Application.Http.HttpResponse();

            UsuariosViewModel usuariosViewModel = new UsuariosViewModel();
            var usuarioServicesMock = new Mock<IUsuarioServices>();
            usuarioServicesMock.Setup(x => x.Remove(usuariosViewModel)).Returns(httpResponse.Response(HttpStatusCode.OK, null, "OK"));
            var controller = new Presentation.Controllers.Usuario(usuarioServicesMock.Object);

            var result = controller.Remove(usuariosViewModel);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            }
        }
    }
}
