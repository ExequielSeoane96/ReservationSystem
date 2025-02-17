using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Reservation_System.Models;

public class Servicio
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Nombre { get; set; }

    [StringLength(255)]
    public string? Descripcion { get; set; } // Descripción opcional

    [Precision(18, 2)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    // Relación con Reservas
    public List<Reserva> Reservas { get; set; } = new();
}
