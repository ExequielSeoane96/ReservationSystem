using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation_System.Data;
using Reservation_System.Models;

[Route("api/[controller]")]
[ApiController]
public class ServiciosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ServiciosController(AppDbContext context)
    {
        _context = context;
    }

    // Obtener todos los servicios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Servicio>>> GetServicios()
    {
        return await _context.Servicios.ToListAsync();
    }

    // Obtener un servicio por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Servicio>> GetServicio(int id)
    {
        var servicio = await _context.Servicios.FindAsync(id);
        if (servicio == null)
        {
            return NotFound();
        }
        return servicio;
    }

    // Crear un nuevo servicio
    [HttpPost]
    public async Task<ActionResult<Servicio>> PostServicio(Servicio servicio)
    {
        _context.Servicios.Add(servicio);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetServicio), new { id = servicio.Id }, servicio);
    }

    // Editar un servicio existente
    [HttpPut("{id}")]
    public async Task<IActionResult> PutServicio(int id, Servicio servicio)
    {
        if (id != servicio.Id)
        {
            return BadRequest();
        }

        _context.Entry(servicio).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Servicios.Any(s => s.Id == id))
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

    // Eliminar un servicio
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServicio(int id)
    {
        var servicio = await _context.Servicios.FindAsync(id);
        if (servicio == null)
        {
            return NotFound();
        }

        _context.Servicios.Remove(servicio);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
