﻿using Microsoft.AspNetCore.Identity;

public class Usuario : IdentityUser
{
    public required string Nombre { get; set; }
}
