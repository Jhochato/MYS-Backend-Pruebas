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
    public class PaisesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PaisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pais>>> GetPaises()
        {
            return await _context.Paises.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pais>> GetPais(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null)
                return NotFound();

            return pais;
        }

        [HttpPost]
        public async Task<ActionResult<Pais>> PostPais(Pais pais)
        {
            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPais), new { id = pais.Id }, pais);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPais(int id, Pais pais)
        {
            if (id != pais.Id)
                return BadRequest("El ID no coincide.");

            _context.Entry(pais).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
                        
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePais(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null)
                return NotFound();

            bool asociaPeliculas = await _context.Peliculas.AnyAsync(p => p.PaisId == id);
            if (asociaPeliculas)
            {
                return BadRequest(new { message = "No se puede eliminar el pais debido a que existen actores, directores y peliculas asociadas a este. " +
                    "Elimine primero Peliculas, y posteriormente directores y actores" });
            }

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
