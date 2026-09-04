using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Globalization;

namespace YaVoy.Services
{
    public class RutaService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://yavoy-ors-proxy.yavoyvalencia.workers.dev/";

        public RutaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Calcula la distancia real en km entre origen y destino
        public async Task<double?> ObtenerDistanciaKmAsync(
            double origenLat, double origenLng,
            double destinoLat, double destinoLng)
        {
            var url = $"{BaseUrl}?" +
                $"start={origenLng.ToString(CultureInfo.InvariantCulture)},{origenLat.ToString(CultureInfo.InvariantCulture)}" +
                $"&end={destinoLng.ToString(CultureInfo.InvariantCulture)},{destinoLat.ToString(CultureInfo.InvariantCulture)}";

            try
            {
                var respuesta = await _httpClient.GetFromJsonAsync<OrsResponse>(url);
                //var metros = respuesta?.Features?.FirstOrDefault()?.Properties?.Summary?.Distance;
                var metros = respuesta?.Features?.FirstOrDefault()?.Properties?.Summary?.Distance;
                if (metros == null) return null;

                return Math.Round(metros.Value / 1000.0, 2); // convertir metros a km
            }
            catch (Exception)
            {
                return null; // manejaremos el mensaje de error en la pantalla
            }
        }

        // Intenta extraer latitud/longitud de un link "largo" de Google Maps
        public (double lat, double lng)? ExtraerCoordenadasDeLink(string link)
        {
            // Busca patrones tipo @10.1621,-68.0077 o q=10.1621,-68.0077
            var patron = new Regex(@"[@=](-?\d+\.\d+),(-?\d+\.\d+)");
            var match = patron.Match(link);

            if (match.Success &&
                double.TryParse(match.Groups[1].Value, out double lat) &&
                double.TryParse(match.Groups[2].Value, out double lng))
            {
                return (lat, lng);
            }

            return null; // no se reconoció (probablemente es un link corto)
        }

        // Clases auxiliares para leer la respuesta JSON de OpenRouteService
        private class OrsResponse
        {
            [JsonPropertyName("features")]
            public List<OrsFeature>? Features { get; set; }
        }

        private class OrsFeature
        {
            [JsonPropertyName("properties")]
            public OrsProperties? Properties { get; set; }
        }

        private class OrsProperties
        {
            [JsonPropertyName("summary")]
            public OrsSummary? Summary { get; set; }
        }

        private class OrsSummary
        {
            [JsonPropertyName("distance")]
            public double Distance { get; set; }
        }

    }
}