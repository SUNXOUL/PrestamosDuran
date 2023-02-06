using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
namespace GestionPrestamosPersonales2023
{
    public class PagosBLL
    {
    private Contexto _contexto;
    public PagosBLL(Contexto contexto)
    {
        _contexto = contexto;
    }

    public bool Guardar(Pago Pago)
    {
        if (!Existe(Pago.PagoId))
            return Insertar(Pago);
        else
            return Modificar(Pago);
    }

    public bool Existe(int PagoId)
    {
        return _contexto.Pago.Any(o => o.PagoId == PagoId);
    }

    private bool Insertar(Pago Pago)
    {
        _contexto.Pago.Add(Pago);
        int cantidad = _contexto.SaveChanges();
        return cantidad > 0;
    }

    public bool Modificar(Pago Pago)
    {
        _contexto.Entry(Pago).State = EntityState.Modified;
        int cantidad = _contexto.SaveChanges();
        _contexto.Entry(Pago).State = EntityState.Detached;
        return cantidad > 0;
    }
    
    public List<Pago> GetPagos()
    {
        return _contexto.Pago.ToList();
    }

        public bool Eliminar(Pago Pago)
        {
            _contexto.Entry(Pago).State=EntityState.Deleted;
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM Pago WHERE PagoId={Pago.PagoId};");
            _contexto.Entry(Pago).State = EntityState.Detached;
            return _contexto.SaveChanges()>0;
        }   

        public Pago? Buscar(int PagoID)
        {
            return _contexto.Pago
                    .Where(o => o.PagoId==PagoID ).AsNoTracking().SingleOrDefault();
                    
        }
        public List<Pago> GetList()
        {
            return _contexto.Pago.AsNoTracking().ToList();
        }
    }
}