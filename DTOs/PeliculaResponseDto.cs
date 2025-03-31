using AdmSeriesAnimadasAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace AdmSeriesAnimadasAPI.DTOs
{
    public class PeliculaResponseDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Reseña { get; set; }
        public string ImagenPortada { get; set; }
        public string CodigoTrailer { get; set; }
        public int GeneroId { get; set; }
        public string Genero { get; set; }
        public int PaisId { get; set; }
        public string Pais { get; set; }
        public int DirectorId { get; set; }
        public string DirectorNombre { get; set; }
        public string DirectorApellido { get; set; }
        public int DirectorPaisId { get; set; }
        public string DirectorPaisNombre { get; set; }
        public List<ActorDto> Actores { get; set; } = new List<ActorDto>();
    }


    public class ActorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
    }
}
