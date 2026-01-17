using Microsoft.EntityFrameworkCore;

namespace server.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>()
                .Property(p => p.NombreCompleto)
                .HasComputedColumnSql("[Nombres] + ' ' + [Apellidos]", stored: true);

            modelBuilder.Entity<Persona>()
                .Property(p => p.IdentificacionCompleta)
                .HasComputedColumnSql("[NumeroIdentificacion] + [TipoIdentificacion]", stored: true);
        }
        public DbSet<Persona> Personas => Set<Persona>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
    }
}
