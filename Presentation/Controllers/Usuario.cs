using Application.Usuario.Interfaces;
using Application.Usuario.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Usuario : Controller
    {
        private readonly IUsuarioServices _usuarioServices;
        public Usuario(IUsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }

        [HttpGet]
        [Authorize]
        [Route("GetList")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuariosViewModel>))]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var result = await _usuarioServices.GetList();
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Authorize]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Register(UsuariosViewModel usuariosViewModel)
        {
            try
            {

                var result = _usuarioServices.Add(usuariosViewModel);

                switch (result.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        return StatusCode(StatusCodes.Status400BadRequest, result.ReasonPhrase);
                    case System.Net.HttpStatusCode.InternalServerError:
                        return StatusCode(StatusCodes.Status500InternalServerError, result.ReasonPhrase);

                    default:
                        return StatusCode(StatusCodes.Status200OK, result.ReasonPhrase);
                }
            }
            catch (Exception)
            {

                return Unauthorized();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetById")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuariosViewModel))]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _usuarioServices.GetById(id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Authorize]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type=typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        public IActionResult Update(UsuariosViewModel usuariosViewModel)
        {
            try
            {
                var result = _usuarioServices.Update(usuariosViewModel);
                switch (result.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return StatusCode(StatusCodes.Status404NotFound, result.ReasonPhrase);

                    default:
                        return StatusCode(StatusCodes.Status200OK);
                }
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("Remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        public IActionResult Remove(UsuariosViewModel usuariosViewModel)
        {
            try
            {
                var result = _usuarioServices.Remove(usuariosViewModel);
                switch (result.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return StatusCode(StatusCodes.Status404NotFound, result.ReasonPhrase);

                    default:
                        return StatusCode(StatusCodes.Status200OK);
                }
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
    }
}
