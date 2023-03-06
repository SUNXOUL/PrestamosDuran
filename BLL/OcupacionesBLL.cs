using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
namespace GestionPrestamosPersonales2023
{
    public class OcupacionesBLL
    {
    private Contexto _contexto;
    public OcupacionesBLL(Contexto contexto)
    {
        _contexto = contexto;
    }

    public bool Guardar(Ocupacion ocupacion)
    {
        if (!Existe(ocupacion.OcupacionId))
            return Insertar(ocupacion);
        else
            return Modificar(ocupacion);
    }

    public bool Existe(int ocupacionId)
    {
        return _contexto.Ocupaciones.Any(o => o.OcupacionId == ocupacionId);
    }

    private bool Insertar(Ocupacion ocupacion)
    {
        _contexto.Ocupaciones.Add(ocupacion);
        int cantidad = _contexto.SaveChanges();
        return cantidad > 0;
    }

    private bool Modificar(Ocupacion ocupacion)
    {
        _contexto.Entry(ocupacion).State = EntityState.Modified;
        int cantidad = _contexto.SaveChanges();
        _contexto.Entry(ocupacion).State = EntityState.Detached;
        return cantidad > 0;
    }
    
    public List<Ocupacion> GetOcupaciones()
    {
        return _contexto.Ocupaciones.ToList();
    }

        public bool Eliminar(Ocupacion ocupacion)
        {
            
            _contexto.Entry(ocupacion).State=EntityState.Deleted;
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM Ocupaciones WHERE OcupacionId={ocupacion.OcupacionId};");
            _contexto.Entry(ocupacion).State = EntityState.Detached;
            return _contexto.SaveChanges()>0;
        }   

        public Ocupacion? Buscar(int ocupacionID)
        {
            return _contexto.Ocupaciones
                    .Where(o => o.OcupacionId==ocupacionID ).AsNoTracking().SingleOrDefault();
                    
        }
        public List<Ocupacion> GetList()
        {
            return _contexto.Ocupaciones.AsNoTracking().ToList();
        }
    }
}