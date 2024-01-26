using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Login.Entities
{
    public class Autenticacao
    {
        public string? token { get; set; }
        public string? name { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
    }
}
