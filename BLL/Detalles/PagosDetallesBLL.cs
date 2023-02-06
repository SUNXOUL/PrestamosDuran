using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
namespace GestionPrestamosPersonales2023
{
    public class PagosDetallesBLL
    {
    private Contexto _contexto;
    public PagosDetallesBLL(Contexto contexto)
    {
        _contexto = contexto;
    }

    public bool Guardar(PagoDetalles PagoDetalles)
    {
        if (!Existe(PagoDetalles.Id))
            return Insertar(PagoDetalles);
        else
            return Modificar(PagoDetalles);
    }

    public bool Existe(int PagoDetallesId)
    {
        return _contexto.PagoDetalles.Any(o => o.Id == PagoDetallesId);
    }

    private bool Insertar(PagoDetalles PagoDetalles)
    {
        _contexto.PagoDetalles.Add(PagoDetalles);
        int cantidad = _contexto.SaveChanges();
        return cantidad > 0;
    }

    public bool Modificar(PagoDetalles PagoDetalles)
    {
        _contexto.Entry(PagoDetalles).State = EntityState.Modified;
        int cantidad = _contexto.SaveChanges();
        _contexto.Entry(PagoDetalles).State = EntityState.Detached;
        return cantidad > 0;
    }
    
    public List<PagoDetalles> GetPagoDetallessDetalles()
    {
        return _contexto.PagoDetalles.ToList();
    }

        public bool Eliminar(PagoDetalles PagoDetalles)
        {
            _contexto.Entry(PagoDetalles).State=EntityState.Deleted;
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM PagoDetalles WHERE Id={PagoDetalles.Id};");
            _contexto.Entry(PagoDetalles).State = EntityState.Detached;
            return _contexto.SaveChanges()>0;
        }   

        public PagoDetalles? Buscar(int PagoDetallesID)
        {
            return _contexto.PagoDetalles
                    .Where(o => o.Id==PagoDetallesID ).AsNoTracking().SingleOrDefault();
                    
        }
        public List<PagoDetalles> GetList()
        {
            return _contexto.PagoDetalles.AsNoTracking().ToList();
        }
    }
}