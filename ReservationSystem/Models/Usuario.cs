namespace Reservation_System.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string Rol { get; set; } = "Cliente"; // Cliente o Admin
    }
}
