using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Context;
using Shop3.Models;

namespace Shop3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orden
        [Authorize(Roles = "administrador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orden>>> GetOrden()
        {
            return await _context.Ordenes.ToListAsync();
        }

        // GET: api/Orden/5
        [Authorize(Roles = "administrador")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Orden>> GetOrden(long id)
        {
            var orden = await _context.Ordenes.FindAsync(id);

            if (orden == null)
            {
                return NotFound();
            }

            return orden;
        }

        // PUT: api/Orden/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrden(long id, Orden orden)
        {
            if (id != orden.Id)
            {
                return BadRequest();
            }

            _context.Entry(orden).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize(Roles = "administrador")]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> Editar(long id, string estado)
        {
            if (OrdenExists(id))
            {
                Orden orden = await _context.Ordenes.FindAsync(id);

                orden.Estado = estado;
                _context.Entry(orden).State = EntityState.Modified;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orden
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "cliente")]
        [HttpPost]
        public async Task<ActionResult<Orden>> PostOrden(Orden orden)
        {
            _context.Ordenes.Add(orden);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrden", new { id = orden.Id }, orden);
        }

        // DELETE: api/Orden/5
        [Authorize(Roles = "administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrden(long id)
        {
            Orden orden = await _context.Ordenes.FindAsync(id);
            if (orden == null)
            {
                return NotFound();
            }
            if (orden.Estado == "confirmed")
            {
                return BadRequest("No puede eliminar esta orden porque esta confirmada");
            }
            _context.Ordenes.Remove(orden);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdenExists(long id)
        {
            return _context.Ordenes.Any(e => e.Id == id);
        }
    }
}
