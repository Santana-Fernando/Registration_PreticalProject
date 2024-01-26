using Application.Login.ViewModel;
using Domain.Login.Entities;
using System.Threading.Tasks;

namespace Application.Login.Interfaces
{
    public interface ILoginServices
    {
        Task<Autenticacao> Login(LoginEntryViewModel login);
    }
}
