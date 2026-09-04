using YaVoy.Models;

namespace YaVoy.Services
{
    public class ZonasService
    {
        private readonly List<Municipio> _municipios = new()
        {
            // Zona Base - Gran Valencia
            new Municipio { Nombre = "Valencia", EsZonaBase = true },
            new Municipio { Nombre = "Naguanagua", EsZonaBase = true },
            new Municipio { Nombre = "San Diego", EsZonaBase = true },
            new Municipio { Nombre = "Los Guayos", EsZonaBase = true },
            new Municipio { Nombre = "Guacara", EsZonaBase = true },
            new Municipio { Nombre = "Tocuyito", EsZonaBase = true },

            // Zona Foránea
            new Municipio { Nombre = "Puerto Cabello", EsZonaBase = false },
            new Municipio { Nombre = "Güigüe", EsZonaBase = false },
            new Municipio { Nombre = "Campo de Carabobo", EsZonaBase = false },
            new Municipio { Nombre = "Maracay", EsZonaBase = false },
            new Municipio { Nombre = "Tinaquillo", EsZonaBase = false }
        };

        public List<Municipio> ObtenerMunicipios() => _municipios;

        public bool EsZonaForanea(string nombreMunicipio)
        {
            var municipio = _municipios.FirstOrDefault(m =>
                m.Nombre.Equals(nombreMunicipio, StringComparison.OrdinalIgnoreCase));
            return municipio != null && !municipio.EsZonaBase;
        }
    }
}