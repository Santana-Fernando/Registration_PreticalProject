using Application.Helpers;
using Application.Http;
using Application.Mapping;
using Application.Usuario.Interfaces;
using Application.Usuario.ViewModel;
using Application.Validation;
using AutoMapper;
using Domain.Usuario.Entidades;
using Domain.Usuario.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Usuario.Services
{
    public class UsuarioServices: IUsuarioServices
    {
        private IUsuario _usuarioRepository;
        private IMapper _mapper;
        public UsuarioServices(IUsuario usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public HttpResponseMessage Add(UsuariosViewModel usuario)
        {
            HttpResponse httpResponse = new HttpResponse();
            var validationFields = new ValidationFields(usuario).IsValidWithErrors();
            List<string> errorMessages = new List<string>(validationFields.ErrorMessages.Select(result => result.ErrorMessage));

            try
            {
                if (!validationFields.IsValid)
                {
                    return httpResponse.Response(HttpStatusCode.BadRequest,
                        new StringContent(JsonSerializer.Serialize(errorMessages)),
                        "Some fields are with invalid values.");
                }

                Usuarios usuarioRegistrado = GetByEmail(usuario.email).Result;
                if (usuarioRegistrado != null)
                {
                    return httpResponse.Response(HttpStatusCode.BadRequest, null, $"There is already a registered user with the email: {usuario.email}.");
                }

                usuario.password = new BCrypter().HashPassword(usuario.password);
                var mapLogin = _mapper.Map<Usuarios>(usuario);
                _usuarioRepository.Add(mapLogin);

                return httpResponse.Response(HttpStatusCode.OK, null, "OK");
            }
            catch (Exception ex)
            {
                return httpResponse.Response(HttpStatusCode.InternalServerError, new StringContent(JsonSerializer.Serialize(ex.Message)), "Internal server error");
            }
        }

        public Task<UsuariosViewModel> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<UsuariosViewModel>> GetList()
        {
            throw new System.NotImplementedException();
        }

        public HttpResponseMessage Remove(UsuariosViewModel usuario)
        {
            throw new System.NotImplementedException();
        }

        public HttpResponseMessage Update(UsuariosViewModel usuario)
        {
            throw new System.NotImplementedException(); 
        }

        private async Task<Usuarios> GetByEmail(string email)
        {
            return await _usuarioRepository.GetByEmail(email);
        }
    }
}
