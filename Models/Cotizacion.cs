namespace YaVoy.Models
{
    public class Cotizacion
    {
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public double DistanciaKm { get; set; }
        public double PrecioUsd { get; set; }
        public double PrecioBs { get; set; }
        public double TasaEuroUsada { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public bool EsZonaForanea { get; set; }
    }
}