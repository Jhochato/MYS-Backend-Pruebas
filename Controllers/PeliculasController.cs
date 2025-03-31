using AdmSeriesAnimadasAPI.Data;
using AdmSeriesAnimadasAPI.DTOs;
using AdmSeriesAnimadasAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmSeriesAnimadasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PeliculasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PeliculaResponseDto>>> GetPeliculas()
        {
            var peliculas = await _context.Peliculas
                .Include(p => p.Genero)
                .Include(p => p.Pais)
                .Include(p => p.Director)
                .Include(p => p.PeliculasActores)
                    .ThenInclude(pa => pa.Actor)
                .ToListAsync();

            var peliculasDto = peliculas.Select(p => new PeliculaResponseDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Reseña = p.Reseña,
                ImagenPortada = p.ImagenPortada,
                CodigoTrailer = p.CodigoTrailer,
                GeneroId = p.Genero.Id,
                Genero = p.Genero.Nombre,
                PaisId = p.Pais.Id,
                Pais = p.Pais.Nombre,
                DirectorId = p.Director.Id,
                DirectorNombre = p.Director.Nombre,
                DirectorApellido = p.Director.Apellidos,
                Actores = p.PeliculasActores.Select(pa => new ActorDto
                {
                    Id = pa.Actor.Id,
                    Nombre = pa.Actor.Nombre,
                    Apellidos = pa.Actor.Apellidos
                }).ToList()
            }).ToList();

            return Ok(peliculasDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaResponseDto>> GetPelicula(int id)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .Include(p => p.Pais)
                .Include(p => p.Director).ThenInclude(d => d.Pais)
                .Include(p => p.PeliculasActores)
                    .ThenInclude(pa => pa.Actor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
                return NotFound();

            var peliculaDto = new PeliculaResponseDto
            {
                Id = pelicula.Id,
                Titulo = pelicula.Titulo,
                Reseña = pelicula.Reseña,
                ImagenPortada = pelicula.ImagenPortada,
                CodigoTrailer = pelicula.CodigoTrailer,
                GeneroId = pelicula.Genero.Id,
                Genero = pelicula.Genero.Nombre,
                PaisId = pelicula.Pais.Id,
                Pais = pelicula.Pais.Nombre,
                DirectorId = pelicula.Director.Id,
                DirectorNombre = pelicula.Director.Nombre,
                DirectorApellido = pelicula.Director.Apellidos,
                DirectorPaisId = pelicula.Director.Pais.Id,
                DirectorPaisNombre = pelicula.Director.Pais.Nombre,
                Actores = pelicula.PeliculasActores.Select(pa => new ActorDto
                {
                    Id = pa.Actor.Id,
                    Nombre = pa.Actor.Nombre,
                    Apellidos = pa.Actor.Apellidos
                }).ToList()
            };

            return Ok(peliculaDto);
        }

        [HttpPost]
        public async Task<ActionResult<Pelicula>> PostPelicula([FromBody] PeliculaCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var generoExiste = await _context.Generos.AnyAsync(g => g.Id == request.Genero.Id);
            var paisExiste = await _context.Paises.AnyAsync(p => p.Id == request.Pais.Id);
            var directorExiste = await _context.Directores.AnyAsync(d => d.Id == request.Director.Id);

            if (!generoExiste)
                return BadRequest($"El género con ID {request.Genero.Id} no existe.");
            if (!paisExiste)
                return BadRequest($"El país con ID {request.Pais.Id} no existe.");
            if (!directorExiste)
                return BadRequest($"El director con ID {request.Director.Id} no existe.");

            var actoresExistentes = await _context.Actores
                .Where(a => request.ActoresSeleccionados.Contains(a.Id))
                .Select(a => a.Id)
                .ToListAsync();

            var actoresNoExistentes = request.ActoresSeleccionados.Except(actoresExistentes).ToList();
            if (actoresNoExistentes.Any())
                return BadRequest($"Los siguientes actores no existen en la base de datos: {string.Join(", ", actoresNoExistentes)}");

            var nuevaPelicula = new Pelicula
            {
                Titulo = request.Titulo,
                Reseña = request.Reseña,
                ImagenPortada = request.ImagenPortada,
                CodigoTrailer = request.CodigoTrailer,
                GeneroId = request.Genero.Id,
                PaisId = request.Pais.Id,
                DirectorId = request.Director.Id,
                PeliculasActores = request.ActoresSeleccionados.Select(actorId => new PeliculaActor
                {
                    ActorId = actorId
                }).ToList()
            };

            _context.Peliculas.Add(nuevaPelicula);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPelicula), new { id = nuevaPelicula.Id }, nuevaPelicula);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelicula(int id, PeliculaUpdateDto peliculaDto)
        {
            if (id != peliculaDto.Id)
                return BadRequest("El ID de la URL no coincide con el ID de la película.");

            try
            {
                var peliculaExistente = await _context.Peliculas
                    .Include(p => p.PeliculasActores)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (peliculaExistente == null)
                    return NotFound("Película no encontrada.");

                peliculaExistente.Titulo = peliculaDto.Titulo;
                peliculaExistente.Reseña = peliculaDto.Reseña;
                peliculaExistente.ImagenPortada = peliculaDto.ImagenPortada;
                peliculaExistente.CodigoTrailer = peliculaDto.CodigoTrailer;
                peliculaExistente.GeneroId = peliculaDto.Genero.Id;
                peliculaExistente.PaisId = peliculaDto.Pais.Id;
                peliculaExistente.DirectorId = peliculaDto.Director.Id;

                if (peliculaDto.ActoresSeleccionados != null && peliculaDto.ActoresSeleccionados.Any())
                {
                    var actoresExistentes = await _context.Actores
                        .Where(a => peliculaDto.ActoresSeleccionados.Contains(a.Id))
                        .Select(a => a.Id)
                        .ToListAsync();

                    if (actoresExistentes.Count != peliculaDto.ActoresSeleccionados.Count)
                        return BadRequest("Uno o más actores no existen en la base de datos.");

                    peliculaExistente.PeliculasActores.Clear();
                    peliculaExistente.PeliculasActores = peliculaDto.ActoresSeleccionados
                        .Select(actorId => new PeliculaActor { PeliculaId = peliculaDto.Id, ActorId = actorId })
                        .ToList();
                }

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar la película: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePelicula(int id)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.PeliculasActores)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
                return NotFound("Película no encontrada.");

            _context.PeliculaActores.RemoveRange(pelicula.PeliculasActores);

            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
