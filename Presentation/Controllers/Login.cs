using Application.Login.Interfaces;
using Application.Login.ViewModel;
using Domain.Login.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    public class Login : Controller
    {
        private readonly ILoginServices _loginServices;
        public Login(ILoginServices loginServices)
        {
            _loginServices = loginServices;
        }

        [HttpPost]
        [Route("Entry")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Autenticacao))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Autenticacao))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Autenticacao))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Autenticacao))]
        public async Task<IActionResult> LoginEntry([FromBody]LoginEntryViewModel loginEntryViewModel)
        {
            var result = await _loginServices.Login(loginEntryViewModel);

            switch (result.statusCode)
            {
                case System.Net.HttpStatusCode.Forbidden:
                    return StatusCode(StatusCodes.Status403Forbidden, result);
                case System.Net.HttpStatusCode.BadRequest:
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                case System.Net.HttpStatusCode.OK:
                    return StatusCode(StatusCodes.Status200OK, result);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            
        }
    }
}
