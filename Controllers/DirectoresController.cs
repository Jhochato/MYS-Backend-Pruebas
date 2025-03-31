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
    public class DirectoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DirectoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Director>>> GetDirectores()
        {
            return await _context.Directores.Include(d => d.Pais).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Director>> GetDirector(int id)
        {
            var director = await _context.Directores.Include(d => d.Pais).FirstOrDefaultAsync(d => d.Id == id);
            if (director == null)
                return NotFound();

            return director;
        }

        [HttpPost]
        public async Task<ActionResult<Director>> PostDirector(Director director)
        {
            var pais = await _context.Paises.FindAsync(director.PaisId);
            
            director.Pais = pais;
            _context.Directores.Add(director);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, director);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirector(int id, Director director)
        {
            if (id != director.Id)
                return BadRequest("El ID no coincide.");

            _context.Entry(director).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            var director = await _context.Directores.FindAsync(id);
            
            bool asociaPeliculas = await _context.Peliculas.AnyAsync(p => p.DirectorId == id);
            if (asociaPeliculas)
            {
                return BadRequest(new { message = "No se puede eliminar el director debido a que tiene peliculas asociadas. Primero debe eliminar las películas." });
            }

            _context.Directores.Remove(director);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

