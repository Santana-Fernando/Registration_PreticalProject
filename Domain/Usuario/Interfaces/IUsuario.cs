using Domain.Login.Entities;
using Domain.Usuario.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Usuario.Interfaces
{
    public interface IUsuario
    {
        void Add(Usuarios usuario);
        Task<IEnumerable<Usuarios>> GetList();
        Task<Usuarios> GetById(int id);
        Task<Usuarios> GetByEmail(string email);
        void Update(Usuarios usuario);
        void Remove(Usuarios usuario);
    }
}
