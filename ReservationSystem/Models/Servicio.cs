using Microsoft.EntityFrameworkCore;

public class Servicio
{
    public int Id { get; set; }
    public required string Nombre { get; set; }

    [Precision(18, 2)]
    public decimal Precio { get; set; }
}
