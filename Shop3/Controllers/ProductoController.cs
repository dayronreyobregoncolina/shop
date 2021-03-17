using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Context;
using Shop3.Models;

namespace Shop3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Rol> _rolManager;

        public ProductoController(ApplicationDbContext context, UserManager<Usuario> userManager, RoleManager<Rol> rolManager)
        {
            _context = context;
            _userManager = userManager;
            _rolManager = rolManager;
        }

        // GET: api/Producto
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        // GET: api/Producto/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(long id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        // PUT: api/Producto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "administrador,vendedor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(long id, Producto producto)
        {
            //Me falto extraer el usuario autenticado para verificar si el producto que va a actualizar es suyo.
            //Se puede hacer con el token que viene de acceso o mandando el id de usuario en el body de la peticion.
            //Una vez tenga el usuario pues verifico si el producto.UsuarioId==authUser.Id

            if (id != producto.Id)
            {
                return BadRequest();
            }


            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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

        // POST: api/Producto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "administrador,vendedor")]
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducto", new { id = producto.Id }, producto);
        }

        // DELETE: api/Producto/5
        [Authorize(Roles = "administrador,vendedor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(long id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
      }

        private bool ProductoExists(long id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }


    }
}
