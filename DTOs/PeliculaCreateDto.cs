using AdmSeriesAnimadasAPI.Models;

namespace AdmSeriesAnimadasAPI.DTOs
{
    public class PeliculaCreateDto
    {
        public string Titulo { get; set; }
        public string Reseña { get; set; }
        public string ImagenPortada { get; set; }
        public string CodigoTrailer { get; set; }
        public int GeneroId { get; set; }
        public Genero Genero { get; set; }
        public int PaisId { get; set; }
        public Pais Pais { get; set; }
        public int DirectorId { get; set; }

        public Director Director { get; set; }
        public List<int> ActoresSeleccionados { get; set; } = new List<int>();
    }
}
