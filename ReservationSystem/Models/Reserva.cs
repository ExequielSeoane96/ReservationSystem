namespace Reservation_System.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public required Usuario Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Confirmada, Cancelada
    }
}
