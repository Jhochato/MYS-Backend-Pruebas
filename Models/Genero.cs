using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmSeriesAnimadasAPI.Models
{
    [Table("Generos")]
    public class Genero
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
    }
}
