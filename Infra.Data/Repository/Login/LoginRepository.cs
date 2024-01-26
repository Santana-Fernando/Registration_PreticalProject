using Domain.Entities;
using Domain.Login.Entities;
using Infra.Data.Context;
using Infra.Data.Context.Helpers;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Application.Http;

namespace Infra.Data.Repository.Login
{
    public class LoginRepository : ILogin
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public LoginRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Autenticacao> Login(LoginEntry login)
        {
            HttpResponse httpResponse = new HttpResponse();
            try
            {
                var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.email == login.email);
                if (usuario != null)
                {
                    if(new BCrypter().VerifyPassword(login.password, usuario.password))
                    {
                        string token = new TokenAuthentication(_configuration).GenerateToken(login.email);
                        return httpResponse.Ok(usuario.name, token);
                    }

                    return httpResponse.Forbidden();
                }

                return httpResponse.Forbidden();
            }
            catch (Exception ex)
            {
                return httpResponse.InternalServerError(ex.Message);
            }
        }
    }
}
