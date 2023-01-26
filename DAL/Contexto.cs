using Microsoft.EntityFrameworkCore;
namespace GestionPrestamosPersonales2023
{
    public class Contexto : DbContext
    {
        public DbSet<Nacionalidades> Nacionalidades {get; set;}
        public DbSet<Estudiantes> Estudiantes {get;set;}
        public DbSet<Tareas> Tareas {get;set;}
        public DbSet<Adicionales> Adicionales {get;set;}
        public DbSet<TiposTelefonos> TiposTelefonos {get;set;}
        public DbSet<Ocupaciones> Ocupaciones {get;set;}
        public Contexto(DbContextOptions<Contexto> Options) : base(Options){}
    }
}