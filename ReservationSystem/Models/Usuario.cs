using Microsoft.AspNetCore.Identity;
using Reservation_System.Models;

public class Usuario : IdentityUser
{
    public required string Nombre { get; set; }
    public List<Reserva> Reservas { get; set; } = new List<Reserva>();
}
