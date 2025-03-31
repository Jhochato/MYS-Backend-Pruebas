using System.ComponentModel.DataAnnotations.Schema;

namespace AdmSeriesAnimadasAPI.Models
{
    [Table("PeliculasActores")]
    public class PeliculaActor
    {
        public int PeliculaId { get; set; }
        public int ActorId { get; set; }

        public virtual Pelicula Pelicula { get; set; }
        public virtual Actor Actor { get; set; }
    }
}
