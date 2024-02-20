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
using Infra.Data.Repository.Usuario;
using Domain.Usuario.Entidades;
using System.Linq;
using System.Collections.Generic;

namespace Tests.Usuario
{
    public class UsuarioRepositoryTest
    {
        private readonly ITestOutputHelper _output;
        private readonly ApplicationDbContext _dbContext;
        private readonly UsuariosRepository _usuariosRepository;
        public UsuarioRepositoryTest(ITestOutputHelper output)
        {
            _output = output;

            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            _dbContext = new ApplicationDbContext(options);
            _usuariosRepository = new UsuariosRepository(_dbContext);
        }

        [Fact]
        public async Task UsuarioRepository_ShouldCallFunctionAdd()
        {
            _output.WriteLine("Should call the function Add");
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var usuario = new Usuarios
            {
                name = "santana",
                email = "santana@gmail.com",
                password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
            };

            try
            {
                _usuariosRepository.Add(usuario);

                var usuarioInserido = await _dbContext.Usuarios.SingleOrDefaultAsync(u => u.email == usuario.email);
                Assert.NotNull(usuarioInserido);
                Assert.True(usuarioInserido.id > 0);

                _usuariosRepository.Remove(usuarioInserido);

                var usuarioRemovido = await _dbContext.Usuarios.SingleOrDefaultAsync(u => u.email == usuario.email);
                Assert.Null(usuarioRemovido);
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task UsuarioRepository_ShouldCallFunctionGetById()
        {
            _output.WriteLine("Should call the function GetById");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var usuario = await _usuariosRepository.GetById(1);

                Assert.NotNull(usuario);
                Assert.True(usuario.email == "fernando@gmail.com");
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task UsuarioRepository_ShouldCallFunctionGetByEmail()
        {
            _output.WriteLine("Should call the function GetByEmail");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var usuario = await _usuariosRepository.GetByEmail("fernando@gmail.com");

                Assert.NotNull(usuario);
                Assert.True(usuario.email == "fernando@gmail.com");
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task UsuarioRepository_ShouldCallFunctionGetList()
        {
            _output.WriteLine("Should call the function GetList");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            var usuarios = new List<Usuarios>
            {
                new Usuarios
                {
                    name = "santana1",
                    email = "santana1@gmail.com",
                    password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
                },
                new Usuarios
                {
                    name = "santana2",
                    email = "santana2@gmail.com",
                    password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
                },
                new Usuarios
                {
                    name = "santana3",
                    email = "santana3@gmail.com",
                    password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
                }
            };

            try
            {
                foreach (var usuarioTeste in usuarios)
                {
                    _usuariosRepository.Add(usuarioTeste);
                }

                var usuario = await _usuariosRepository.GetList();

                Assert.NotNull(usuario);
                Assert.True(usuario.ToList().Count == 4);

                foreach (var usuarioTeste in usuarios)
                {
                    _usuariosRepository.Remove(usuarioTeste);
                }
            }
            finally
            {
                await transaction.RollbackAsync();
            }

        }

        [Fact]
        public async Task UsuarioRepository_ShouldCallFunctionUpdate()
        {
            _output.WriteLine("Should call the function Update");
            string emailTeste = "fernandorodriguesdesantana@gmail.com";
            string nomeTeste = "Fernando Rodrigues";

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var usuario = new Usuarios
            {
                name = "santana",
                email = "santana@gmail.com",
                password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
            };

            try
            {
                _usuariosRepository.Add(usuario);

                var usuarioInserido = await _dbContext.Usuarios.SingleOrDefaultAsync(u => u.email == usuario.email);

                Assert.NotNull(usuarioInserido);
                Assert.True(usuarioInserido.email == "santana@gmail.com");

                usuarioInserido.email = emailTeste;
                usuarioInserido.name = nomeTeste;

                _usuariosRepository.Update(usuarioInserido);

                var usuarioModificado = await _dbContext.Usuarios.SingleOrDefaultAsync(u => u.email == emailTeste);

                Assert.NotNull(usuarioModificado);
                Assert.True(usuarioModificado.email == emailTeste);
                Assert.True(usuarioModificado.name == nomeTeste);

                _usuariosRepository.Remove(usuarioModificado);
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task UsuarioRepository_ShouldCallFunctionRemove()
        {
            _output.WriteLine("Should call the function Remove");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var usuario = new Usuarios
            {
                name = "santana",
                email = "santana@gmail.com",
                password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
            };

            try
            {
                _usuariosRepository.Add(usuario);

                var usuarioInserido = await _dbContext.Usuarios.SingleOrDefaultAsync(u => u.email == usuario.email);

                Assert.NotNull(usuarioInserido);
                Assert.True(usuarioInserido.email == usuario.email);

                _usuariosRepository.Remove(usuarioInserido);

                var usuarioRemovido = await _dbContext.Usuarios.SingleOrDefaultAsync(u => u.email == usuario.email);
                Assert.Null(usuarioRemovido);
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
