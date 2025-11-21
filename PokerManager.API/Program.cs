using Microsoft.EntityFrameworkCore;
using PokerManager.API.Data;
using PokerManager.API.Services;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------
// 1. CONFIGURACIÓN DE SERVICIOS
// --------------------------------------------------------

// A. Base de Datos (SQL Server)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// B. Inyección de Dependencias (Tus Servicios)
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GameService>(); // Nota: Crea el archivo GameService.cs para que esto funcione

// C. Controladores y API
builder.Services.AddControllers();

// D. Swagger (Documentación)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --------------------------------------------------------
// 2. CONSTRUCCIÓN DE LA APP
// --------------------------------------------------------
var app = builder.Build();

// A. Configurar Swagger (Siempre visible, incluso en Azure)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokerManager API v1");
    // Esto hace que Swagger se abra en la raíz (https://tu-web.azurewebsites.net/)
    c.RoutePrefix = string.Empty; 
});

// B. Middleware estándar
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();