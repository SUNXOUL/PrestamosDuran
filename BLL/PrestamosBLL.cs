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
        return _contexto.Prestamo.Any(o => o.PrestamoId == PrestamoId);
    }

    private bool Insertar(Prestamo Prestamo)
    {
        _contexto.Prestamo.Add(Prestamo);
        int cantidad = _contexto.SaveChanges();
        return cantidad > 0;
    }

    private bool Modificar(Prestamo Prestamo)
    {
        _contexto.Entry(Prestamo).State = EntityState.Modified;
        int cantidad = _contexto.SaveChanges();
        _contexto.Entry(Prestamo).State = EntityState.Detached;
        return cantidad > 0;
    }
    
    public List<Prestamo> GetPrestamos()
    {
        return _contexto.Prestamo.ToList();
    }

        public bool Eliminar(Prestamo Prestamo)
        {
            _contexto.Entry(Prestamo).State=EntityState.Deleted;
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM Prestamo WHERE PrestamoId={Prestamo.PrestamoId};");
            _contexto.Entry(Prestamo).State = EntityState.Detached;
            return _contexto.SaveChanges()>0;
        }   

        public Prestamo? Buscar(int PrestamoID)
        {
            return _contexto.Prestamo
                    .Where(o => o.PrestamoId==PrestamoID ).AsNoTracking().SingleOrDefault();
                    
        }
        public List<Prestamo> GetList()
        {
            return _contexto.Prestamo.AsNoTracking().ToList();
        }
    }
}