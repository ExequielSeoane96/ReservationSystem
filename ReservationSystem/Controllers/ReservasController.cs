using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation_System.Data;
using Reservation_System.Models;
using System.Security.Claims;

namespace Reservation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _context.Reservas.Include(r => r.Usuario).Include(r => r.Servicio).ToListAsync();
        }

        [HttpPost]
        [Authorize]  // Requiere autenticación (JWT)
        public async Task<ActionResult<Reserva>> CrearReserva([FromBody] Reserva reserva)
        {
            if (reserva == null)
            {
                return BadRequest("Reserva inválida.");
            }

            // Obtener el usuarioId desde el token JWT
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioIdClaim))
            {
                return Unauthorized("No se pudo obtener el usuario del token.");
            }

            // Asignar el usuarioId a la reserva
            reserva.UsuarioId = usuarioIdClaim;

            // Verificar si el horario está disponible
            bool existeReserva = await _context.Reservas
                .AnyAsync(r => r.FechaHora == reserva.FechaHora && r.ServicioId == reserva.ServicioId);

            if (existeReserva)
            {
                return BadRequest(new { mensaje = "El horario seleccionado ya está reservado." });
            }

            // Agregar la nueva reserva
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // Retornar la respuesta con la nueva reserva
            return CreatedAtAction(nameof(GetReservas), new { id = reserva.Id }, reserva);
        }

        // GET: api/reservas/validar?fecha=2025-02-20T15:00:00&servicioId=1
        [HttpGet("validar")]
        public async Task<ActionResult<bool>> ValidarDisponibilidad(DateTime fecha, int servicioId)
        {
            bool disponible = !await _context.Reservas
                .AnyAsync(r => r.FechaHora == fecha && r.ServicioId == servicioId);
            return Ok(disponible);
        }

        // PUT: api/reservas/{id}
        [HttpPut("{id}")]
        [Authorize]  // Requiere autenticación (JWT)
        public async Task<IActionResult> ModificarReserva(int id, Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return BadRequest("ID de reserva no coincide.");
            }

            // Obtener el usuarioId desde el token JWT
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioIdClaim))
            {
                return Unauthorized("No se pudo obtener el usuario del token.");
            }

            // Verificar que la reserva pertenece al usuario autenticado
            var reservaExistente = await _context.Reservas.FindAsync(id);
            if (reservaExistente == null || reservaExistente.UsuarioId != usuarioIdClaim)
            {
                return NotFound("Reserva no encontrada o no pertenece al usuario.");
            }

            // Actualizar la reserva
            reservaExistente.FechaHora = reserva.FechaHora;
            reservaExistente.ServicioId = reserva.ServicioId;
            reservaExistente.Estado = reserva.Estado;
            reservaExistente.Detalles = reserva.Detalles;
            reservaExistente.Comentarios = reserva.Comentarios;

            _context.Entry(reservaExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
                {
                    return NotFound("Reserva no encontrada.");
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { mensaje = "Cambio realizado con éxito" });
        }

        // DELETE: api/reservas/{id}
        [HttpDelete("{id}")]
        [Authorize]  // Requiere autenticación (JWT)
        public async Task<IActionResult> BorrarReserva(int id)
        {
            // Obtener el usuarioId desde el token JWT
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioIdClaim))
            {
                return Unauthorized("No se pudo obtener el usuario del token.");
            }

            // Verificar que la reserva pertenece al usuario autenticado
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null || reserva.UsuarioId != usuarioIdClaim)
            {
                return NotFound("Reserva no encontrada o no pertenece al usuario.");
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Reserva borrada con éxito" });
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}
