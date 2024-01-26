using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Context.Helpers
{
    public class BCrypter
    {
        public string HashPassword(string password)
        {
            // Gere um novo salt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hash a senha com o salt
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Use a função Verify do BCrypt para comparar a senha fornecida com o hash armazenado
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }
}
