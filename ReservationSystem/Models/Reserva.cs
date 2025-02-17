using System.ComponentModel.DataAnnotations;

namespace Reservation_System.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public required Usuario Usuario { get; set; }

        [Required]
        public int ServicioId { get; set; }  // Relación con el servicio reservado
        public required Servicio Servicio { get; set; }

        [Required]
        public DateTime FechaHora { get; set; } // Fecha y hora de la reserva

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente"; // Estados posibles: Pendiente, Confirmada, Cancelada
    }
}
