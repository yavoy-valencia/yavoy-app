using System.Text.Json;

namespace YaVoy.Services
{
    public class TasaEuroService
    {
        private readonly HttpClient _httpClient;

        // Fuente principal
        private const string UrlPrincipal = "https://pydolarvenezuela-api.vercel.app/api/v1/euro?page=bcv";
        // Fuente de respaldo, por si la principal falla
        private const string UrlRespaldo = "https://bcv.today/api/v1/rate.json";

        public TasaEuroService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double?> ObtenerTasaEuroAsync()
        {
            // Intento 1: fuente principal
            var tasa = await IntentarObtenerDePyDolar();
            if (tasa != null) return tasa;

            // Intento 2: fuente de respaldo
            tasa = await IntentarObtenerDeBcvToday();
            return tasa;
        }

        private async Task<double?> IntentarObtenerDePyDolar()
        {
            try
            {
                var respuesta = await _httpClient.GetStringAsync(UrlPrincipal);
                using var doc = JsonDocument.Parse(respuesta);

                // La estructura de pyDolarVenezuela anida el valor dentro de "price"
                if (doc.RootElement.TryGetProperty("price", out var precio))
                {
                    return precio.GetDouble();
                }
                return null;
            }
            catch
            {
                return null; // si falla, intentamos la siguiente fuente
            }
        }

        private async Task<double?> IntentarObtenerDeBcvToday()
        {
            try
            {
                var respuesta = await _httpClient.GetStringAsync(UrlRespaldo);
                using var doc = JsonDocument.Parse(respuesta);

                if (doc.RootElement.TryGetProperty("EUR", out var euro))
                {
                    return euro.GetDouble();
                }
                return null;
            }
            catch
            {
                return null; // si ambas fallan, la app pedirá ingreso manual
            }
        }
    }
}