using Microsoft.EntityFrameworkCore;
using server.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

// EntityFramework
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    Console.WriteLine("Looking for Usuarios");
    var areThereUsuarios = await db.Usuarios.AnyAsync();
    Console.WriteLine($"AreThereUsuarios: {areThereUsuarios}");
    if (!await db.Usuarios.AnyAsync())
    {
        db.Usuarios.Add(new Usuario()
        {
            User = "admin",
            Pass = "Pass123*",
            FechaCreacion = DateTime.Now
        });
        await db.SaveChangesAsync();
    };
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
