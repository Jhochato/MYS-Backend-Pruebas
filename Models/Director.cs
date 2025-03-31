using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmSeriesAnimadasAPI.Models
{
    [Table("Directores")]
    public class Director
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Apellidos { get; set; }

        [Required]
        public int PaisId { get; set; }

        public virtual Pais Pais { get; set; }

        public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
    }
}

