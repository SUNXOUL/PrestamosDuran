using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
namespace GestionPrestamosPersonales2023
{
    public class PrestamosBLL
    {
    private Contexto _contexto;
    public PrestamosBLL(Contexto contexto)
    {
        _contexto = contexto;
    }

    public bool Guardar(Prestamo Prestamo)
    {
        if (!Existe(Prestamo.PrestamoId))
            return Insertar(Prestamo);
        else
            return Modificar(Prestamo);
    }

    public bool Existe(int PrestamoId)
    {
        return _contexto.Prestamos.Any(o => o.PrestamoId == PrestamoId);
    }

    private bool Insertar(Prestamo Prestamo)
    {
        _contexto.Prestamos.Add(Prestamo);
        int cantidad = _contexto.SaveChanges();
        return cantidad > 0;
    }

    private bool Modificar(Prestamo Prestamo)
    {
        _contexto.Entry(Prestamo).State = EntityState.Modified;
        int cantidad = _contexto.SaveChanges();
        return cantidad > 0;
    }
    
    public List<Prestamo> GetPrestamos()
    {
        return _contexto.Prestamos.ToList();
    }

        public bool Eliminar(Prestamo Prestamo)
        {
            Console.WriteLine("eliminado");
            _contexto.Entry(Prestamo).State=EntityState.Deleted;
            return _contexto.SaveChanges()>0;
        }   

        public Prestamo? Buscar(int PrestamoID)
        {
            return _contexto.Prestamos
                    .Where(o => o.PrestamoId==PrestamoID ).AsNoTracking().SingleOrDefault();
                    
        }
        public List<Prestamo> GetList()
        {
            return _contexto.Prestamos.AsNoTracking().ToList();
        }
    }
}