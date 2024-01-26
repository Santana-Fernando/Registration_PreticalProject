using Domain.Usuario.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.EntityConfigurations.Usuario
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.Property(p => p.email).HasMaxLength(100).IsRequired();
            builder.Property(p => p.name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.password).HasMaxLength(10).IsRequired();

            builder.HasData(
                new Usuarios
                {
                    id = 1,
                    name = "Fernando",
                    email = "fernando@gmail.com",
                    password = "$2a$10$e/IZDBCPryoa6XMwowkItuVWAeZmYOH1RiinVrcHVTm560uGIaUa2"
                }
           );
        }
    }
}
