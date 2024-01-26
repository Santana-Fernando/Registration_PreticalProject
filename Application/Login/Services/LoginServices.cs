using Application.Login.Interfaces;
using Application.Login.ViewModel;
using Application.Validation;
using Application.Http;
using AutoMapper;
using Domain.Entities;
using Domain.Login.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Application.Login.Services
{
    public class LoginServices : ILoginServices
    {
        private ILogin _loginRepository;
        private IMapper _mapper;
        public LoginServices(ILogin loginRepository, IMapper mapper)
        {
            _loginRepository = loginRepository;
            _mapper = mapper;
        }

        public async Task<Autenticacao> Login(LoginEntryViewModel login)
        {
            HttpResponse httpResponse = new HttpResponse();
            var validationFields = new ValidationFields(login).IsValidWithErrors();
            List<string> errorMessages = new List<string>(validationFields.ErrorMessages.Select(result => result.ErrorMessage));

            if (!validationFields.IsValid)
            {
                return httpResponse.BadRequest(errorMessages);
            }

            var mapLogin = _mapper.Map<LoginEntry>(login);
            return await _loginRepository.Login(mapLogin);
        }
    }
}
