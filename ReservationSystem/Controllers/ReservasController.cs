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
        public async Task<ActionResult<Reserva>> CrearReserva(Reserva reserva)
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

            if (int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return BadRequest("Formato de usuarioId inválido.");
            }

            // Asignar el usuarioId a la reserva
            reserva.UsuarioId = usuarioId;

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
    }
}
