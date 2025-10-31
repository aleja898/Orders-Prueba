using System.Net;

namespace Orders.Frontend.Repositories
{
    public class HttpResponseWrapper<T>(T? response, bool error, HttpResponseMessage httpResponseMessage)
    {
        public T? Response { get; } = response;
        public bool Error { get; } = error;
        public HttpResponseMessage HttpResponseMessage { get; } = httpResponseMessage;

        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            var statusCode = HttpResponseMessage.StatusCode;
            if (statusCode == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado.";
            }
            if (statusCode == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                return "Tienes que estar logueado para ejecutar esta operación.";
            }
            if (statusCode == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para hacer esta operación.";
            }

            return "Ha ocurrido un error inesperado.";
        }
    }
}
