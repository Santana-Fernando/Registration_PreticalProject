using Domain.Login.Entities;
using Domain.Usuario.Entidades;
using System.Threading.Tasks;

namespace Domain.Usuario.Interfaces
{
    public interface IUsuario
    {
        Task<Usuarios> GetUsuarioAsync(LoginEntry loginEntry);
    }
}
