using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required] public required string Nombre { get; set; }
    [Required, EmailAddress] public required string Email { get; set; }
    [Required, MinLength(6)] public required string Password { get; set; }
}

public class LoginRequest
{
    [Required, EmailAddress] public required string Email { get; set; }
    [Required] public required string Password { get; set; }
}