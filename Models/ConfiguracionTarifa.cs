namespace YaVoy.Models
{
    public class ConfiguracionTarifa
    {
        public double TarifaBaseUsd { get; set; } = 1.00;
        public double TarifaPorKmUsd { get; set; } = 0.30;
        public double RecargoZonaForanea { get; set; } = 0.25; // 25% adicional
        public double TasaEuroActual { get; set; } = 0; // la actualizas tú manualmente
        public DateTime FechaActualizacionTasa { get; set; } = DateTime.Now;
    }
}