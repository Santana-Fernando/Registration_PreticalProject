using Application.Http;
using Application.Usuario.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Application.Usuario.Interfaces
{
    public interface IUsuarioServices
    {
        HttpResponseMessage Add(UsuariosViewModel usuario);
        Task<IEnumerable<UsuariosViewModel>> GetList();
        Task<UsuariosViewModel> GetById(int id);
        HttpResponseMessage Update(UsuariosViewModel usuario);
        HttpResponseMessage Remove(UsuariosViewModel usuario);
    }
}
