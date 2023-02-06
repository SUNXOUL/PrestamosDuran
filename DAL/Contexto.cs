using Microsoft.EntityFrameworkCore;
namespace GestionPrestamosPersonales2023
{
    public class Contexto : DbContext
    {
        public DbSet<Nacionalidades> Nacionalidade {get; set;}
        public DbSet<Estudiantes> Estudiante {get;set;}
        public DbSet<Tareas> Tareas {get;set;}
        public DbSet<Adicionales> Adicionale {get;set;}
        public DbSet<TiposTelefonos> TiposTelefonos {get;set;}
        public DbSet<Ocupaciones> Ocupaciones {get;set;}
        public DbSet<Persona> Persona {get;set;}
        public DbSet<Prestamo> Prestamo {get;set;}
        public DbSet<Pago> Pago {get;set;}
        public DbSet<PagoDetalles> PagoDetalles{get;set;}
        public Contexto(DbContextOptions<Contexto> Options) : base(Options){}
    }
}