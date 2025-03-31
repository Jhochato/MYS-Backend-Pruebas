using AdmSeriesAnimadasAPI.Data;
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
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GenerosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genero>>> GetGeneros()
        {
            return await _context.Generos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genero>> GetGenero(int id)
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
                return NotFound();

            return genero;
        }

        [HttpPost]
        public async Task<ActionResult<Genero>> PostGenero(Genero genero)
        {
            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGenero), new { id = genero.Id }, genero);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenero(int id, Genero genero)
        {
            if (id != genero.Id)
                return BadRequest("El ID no coincide.");

            _context.Entry(genero).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenero(int id)
        {
            var genero = await _context.Generos.FindAsync(id);
           
            bool asociaPeliculas = await _context.Peliculas.AnyAsync(p => p.GeneroId == id);
            if (asociaPeliculas)
            {
                return BadRequest(new { message = "No se puede eliminar el genero debido a que existen peliculas asociadas a este. Primero debe eliminar las películas." });
            }

            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

