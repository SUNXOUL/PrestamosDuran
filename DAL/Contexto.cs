using Microsoft.EntityFrameworkCore;
namespace GestionPrestamosPersonales2023
{
    public class Contexto : DbContext
    {
        public DbSet<Ocupacion> Ocupaciones {get;set;}
        public DbSet<Persona> Personas {get;set;}
        public DbSet<Prestamo> Prestamos {get;set;}
        public DbSet<Pago> Pagos {get;set;}

        public Contexto(DbContextOptions<Contexto> Options) : base(Options){}
    }
    
}