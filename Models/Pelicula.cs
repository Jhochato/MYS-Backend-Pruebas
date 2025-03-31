using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AdmSeriesAnimadasAPI.Models
{
    [Table("Peliculas")]
    public class Pelicula
    {
        public int Id { get; set; }
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

        public virtual ICollection<PeliculaActor> PeliculasActores { get; set; } = new List<PeliculaActor>();
    }

}

