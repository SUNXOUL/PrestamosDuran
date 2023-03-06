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

            //Agregamos el monto del balance de la persona
            var persona = _contexto.Personas.Find(Prestamo.PersonaId);
            persona.Balance += Prestamo.Monto;
            _contexto.Entry(persona).State = EntityState.Modified;
            _contexto.SaveChanges();
            _contexto.Entry(persona).State = EntityState.Detached;

            //Agregamos el prestamo 
            _contexto.Prestamos.Add(Prestamo);
            int cantidad = _contexto.SaveChanges();

            return cantidad > 0;
        }

        private bool Modificar(Prestamo Prestamo)
        {
            //Modificamos el monto del balance de la persona
            Prestamo anterior = Buscar(Prestamo.PrestamoId);
            if (anterior.Monto != Prestamo.Monto)
            {
                var persona = _contexto.Personas.Where(o => o.PersonaId == Prestamo.PersonaId).AsNoTracking().SingleOrDefault();
                persona.Balance -= anterior.Monto;
                persona.Balance += Prestamo.Monto;
                _contexto.Entry(persona).State = EntityState.Modified;
                _contexto.SaveChanges();
                _contexto.Entry(persona).State = EntityState.Detached;
            }


            //modificamos el prestamo
            _contexto.Entry(Prestamo).State = EntityState.Modified;
            int cantidad = _contexto.SaveChanges();
            _contexto.Entry(Prestamo).State = EntityState.Detached;
            return cantidad > 0;
        }

        public List<Prestamo> GetPrestamos()
        {
            return _contexto.Prestamos.ToList();
        }

        public bool Eliminar(Prestamo Prestamo)
        {
            //eliminamos el monto del balance de la persona
            var persona = _contexto.Personas.Find(Prestamo.PersonaId);
            persona.Balance -= Prestamo.Monto;
            _contexto.Entry(persona).State = EntityState.Modified;
            _contexto.SaveChanges();
            _contexto.Entry(persona).State = EntityState.Detached;

            //eliminamos el prestamo
            _contexto.Entry(Prestamo).State = EntityState.Deleted;
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM Prestamos WHERE PrestamoId={Prestamo.PrestamoId};");
            _contexto.Entry(Prestamo).State = EntityState.Detached;
            return _contexto.SaveChanges() > 0;
        }

        public Prestamo? Buscar(int PrestamoID)
        {
            return _contexto.Prestamos
                    .Where(o => o.PrestamoId == PrestamoID).AsNoTracking().SingleOrDefault();

        }
        public List<Prestamo> GetList()
        {
            return _contexto.Prestamos.AsNoTracking().ToList();
        }
    }
}