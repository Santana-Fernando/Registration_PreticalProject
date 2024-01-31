using Xunit.Abstractions;
using Xunit;
using Infra.Data.Repository.Login;
using Microsoft.Extensions.Configuration;
using Domain.Login.Entities;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Moq;
using Infra.Data.Context.Helpers;
using Tests.Helper;
using Infra.Data.Repository.Usuario;
using Domain.Usuario.Entidades;
using System.Linq;

namespace Tests.Usuario
{
    public class UsuarioRepositoryTest
    {
        private readonly ITestOutputHelper _output;
        public UsuarioRepositoryTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UsuarioRepository_ShouldCallFunctionAdd()
        {
            _output.WriteLine("Should call the function Add");

            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();

            using var dbContext = new ApplicationDbContext(options);
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            UsuariosRepository usuariosRepository = new UsuariosRepository(dbContext);
            var usuario = new Usuarios
            {
                name = "santana",
                email = "santana@gmail.com",
                password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
            };

            try
            {
                usuariosRepository.Add(usuario);

                var usuarioInserido = await dbContext.Usuarios.SingleOrDefaultAsync(u => u.email == usuario.email);
                Assert.NotNull(usuarioInserido);
                Assert.True(usuarioInserido.id > 0);

                usuariosRepository.Remove(usuarioInserido);

                var usuarioRemovido = await dbContext.Usuarios.SingleOrDefaultAsync(u => u.email == usuario.email);
                Assert.Null(usuarioRemovido);
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
