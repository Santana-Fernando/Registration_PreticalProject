using Domain.Login.Entities;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public interface ILogin
    {
        Task<Autenticacao> Login(LoginEntry login);
    }
}
