🎟 Reservation System
Sistema de gestión de reservas desarrollado con ASP.NET Core, Entity Framework Core y JWT para autenticación.

🚀 Características

📌 Autenticación y autorización con JWT
📅 Gestión de reservas (crear y validar disponibilidad)
🔍 Consulta de reservas con filtrado
🔗 Integración con Entity Framework Core y base de datos SQL Server

🛠 Tecnologías
Backend: ASP.NET Core 6 / 7
ORM: Entity Framework Core
Base de datos: SQL Server
Autenticación: JWT (JSON Web Tokens)
Herramientas: Visual Studio, Postman (para pruebas de API)

⚡ Instalación y configuración
1️⃣ Clonar el repositorio
git clone https://github.com/ExequielSeoane96/ReservationSystem.git
2️⃣ Configurar la base de datos
En appsettings.json, asegúrate de configurar correctamente la cadena de conexión a tu base de datos SQL Server:

"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=ReservationDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}

Luego, aplica las migraciones y actualiza la base de datos:
dotnet ef migrations add InitialCreate
dotnet ef database update

3️⃣ Ejecutar el servidor
dotnet run
La API estará disponible en:
http://localhost:5000 (para HTTP)
https://localhost:5001 (para HTTPS)

📌 Endpoints de la API

📌 Autenticación
POST /api/auth/login → Iniciar sesión y obtener un token JWT
POST /api/auth/register → Registrar un nuevo usuario

📌 Reservas
GET /api/reservas → Obtener todas las reservas
POST /api/reservas → Crear una nueva reserva (requiere autenticación)
GET /api/reservas/validar?fecha=YYYY-MM-DDTHH:mm:ss&servicioId=X → Validar disponibilidad


