using Biblioteca.Data;
using Biblioteca.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Conexão com o banco
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registrar serviços concretos (sem interface)
builder.Services.AddScoped<ClienteService>();


// Controllers + configuração JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Evita ciclos de referência, muito comum em EF
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger apenas em dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Mapeia os controllers automaticamente
app.MapControllers();

app.Run();
