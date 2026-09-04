namespace YaVoy.Models
{
    public class Municipio
    {
        public string Nombre { get; set; } = string.Empty;
        public bool EsZonaBase { get; set; } // true = Gran Valencia, false = foránea
    }
}