using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
namespace GestionPrestamosPersonales2023
{
    public class OcupacionesBLL
    {
        private Contexto _Contexto;
        public OcupacionesBLL(Contexto contexto)
        {
            this._Contexto = contexto;
        }
        public bool Existe(int ocupacionID){
            return _Contexto.Ocupaciones
                    .Any(o=> o.OcupacionId== ocupacionID);
        }

        private bool Insertar(Ocupaciones ocupacion)
        {
            _Contexto.Ocupaciones.Add(ocupacion);
            return _Contexto.SaveChanges()>0;
        }

        public  bool Modificar(Ocupaciones ocupacion)
        {
            _Contexto.Entry(ocupacion).State=Microsoft.EntityFrameworkCore.EntityState.Modified;
            return _Contexto.SaveChanges() >0;
        }

        public  bool Guardar(Ocupaciones ocupacion)
        {
            if(!Existe(ocupacion.OcupacionId))
            {
                return this.Insertar(ocupacion);
            }
            else
            {
                return this.Modificar(ocupacion);
            }
        }

        public bool Eliminar(Ocupaciones ocupacion)
        {
            _Contexto.Entry(ocupacion).State=Microsoft.EntityFrameworkCore.EntityState.Deleted;
            return _Contexto.SaveChanges()>0;
        }

        public Ocupaciones? Buscar(int ocupacionID)
        {
            return _Contexto.Ocupaciones
                    .Where(o => o.OcupacionId==ocupacionID).AsNoTracking().SingleOrDefault();
                    
        }
        public List<Ocupaciones> GetList(Expression<Func<Ocupaciones,bool>>criterio)
        {
            return _Contexto.Ocupaciones
                .AsNoTracking()
                .Where(criterio)
                .ToList();
        }


        
    }
}