using Microsoft.EntityFrameworkCore;
using AdmSeriesAnimadasAPI.Models;

namespace AdmSeriesAnimadasAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Director> Directores { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PeliculaActor> PeliculaActores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pelicula>()
                .HasOne(p => p.Genero)
                .WithMany(g => g.Peliculas)
                .HasForeignKey(p => p.GeneroId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Pelicula>()
                .HasOne(p => p.Pais)
                .WithMany(pa => pa.Peliculas)
                .HasForeignKey(p => p.PaisId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Pelicula>()
                .HasOne(p => p.Director)
                .WithMany(d => d.Peliculas)
                .HasForeignKey(p => p.DirectorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PeliculaActor>()
                .HasKey(pa => new { pa.PeliculaId, pa.ActorId });

            modelBuilder.Entity<PeliculaActor>()
                .HasOne(pa => pa.Pelicula)
                .WithMany(p => p.PeliculasActores)
                .HasForeignKey(pa => pa.PeliculaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PeliculaActor>()
                .HasOne(pa => pa.Actor)
                .WithMany(a => a.PeliculasActores)
                .HasForeignKey(pa => pa.ActorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}


