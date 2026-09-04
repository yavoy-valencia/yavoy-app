using YaVoy.Models;

namespace YaVoy.Services
{
    public class TarifaService
    {
        // Configuración editable - en el próximo paso la conectamos a una pantalla
        public ConfiguracionTarifa Configuracion { get; set; } = new ConfiguracionTarifa();

        public Cotizacion CalcularCotizacion(
            string origen,
            string destino,
            double distanciaKm,
            bool esZonaForanea)
        {
            double precioUsd = Configuracion.TarifaBaseUsd + (distanciaKm * Configuracion.TarifaPorKmUsd);

            if (esZonaForanea)
            {
                precioUsd += precioUsd * Configuracion.RecargoZonaForanea;
            }

            precioUsd = Math.Round(precioUsd, 2);

            double precioBs = Math.Round(precioUsd * Configuracion.TasaEuroActual, 2);

            return new Cotizacion
            {
                Origen = origen,
                Destino = destino,
                DistanciaKm = distanciaKm,
                PrecioUsd = precioUsd,
                PrecioBs = precioBs,
                TasaEuroUsada = Configuracion.TasaEuroActual,
                EsZonaForanea = esZonaForanea,
                FechaHora = DateTime.Now
            };
        }
    }
}