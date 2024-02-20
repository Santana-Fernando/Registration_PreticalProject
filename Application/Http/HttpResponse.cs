using Domain.Login.Entities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Http
{
    public class HttpResponse
    {
        public Autenticacao Ok(string name, string token)
        {
            return new Autenticacao()
            {
                name = name,
                token = token,
                statusCode = System.Net.HttpStatusCode.OK,
                message = "OK"
            };
        }

        public Autenticacao Forbidden()
        {
            return new Autenticacao()
            {
                statusCode = System.Net.HttpStatusCode.Forbidden,
                message = "Access denied"
            };
        }

        public Autenticacao BadRequest(List<string> fields)
        {
            return new Autenticacao()
            {
                statusCode = System.Net.HttpStatusCode.BadRequest,
                message = $"{string.Join(", ", fields)}"
            };
        }

        public Autenticacao NotFound()
        {
            return new Autenticacao()
            {
                statusCode = HttpStatusCode.NotFound,
                message = "Not Found"
            };
        }

        public Autenticacao InternalServerError(string message)
        {
            return new Autenticacao()
            {
                statusCode = HttpStatusCode.InternalServerError,
                message = message
            };
        }

        public HttpResponseMessage Response(HttpStatusCode status, StringContent? messages, string reasonPhrase)
        {
            return new HttpResponseMessage(status)
            {
                Content = new StringContent(JsonSerializer.Serialize(messages)),
                ReasonPhrase = reasonPhrase
            };
        }
    }
}
