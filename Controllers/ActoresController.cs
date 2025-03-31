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
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ActoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActores()
        {
            return await _context.Actores.Include(a => a.Pais).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
            var actor = await _context.Actores.Include(a => a.Pais).FirstOrDefaultAsync(a => a.Id == id);
            if (actor == null)
                return NotFound();

            return actor;
        }

        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor(Actor actor)
        {
            var pais = await _context.Paises.FindAsync(actor.PaisId);
            
            actor.Pais = pais;
            _context.Actores.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int id, Actor actor)
        {
            if (id != actor.Id)
                return BadRequest("El ID no coincide.");

            _context.Entry(actor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var actor = await _context.Actores
                .Include(a => a.PeliculasActores)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            bool tienePeliculas = await _context.PeliculaActores.AnyAsync(pa => pa.ActorId == id);
            if (tienePeliculas)
            {
                return BadRequest(new { message = "No se puede eliminar el actor debido a que tiene películas asociadas. Primero debe eliminar la relación con las películas." });
            }

            _context.Actores.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
