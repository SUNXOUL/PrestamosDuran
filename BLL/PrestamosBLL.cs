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
            //modificamos  a la persona
            var Persona = _contexto.Personas.Find(Prestamo.PersonaId);
            if (Persona != null)
            {
                Persona.Balance += Prestamo.Monto;
                Prestamo.Balance = Prestamo.Monto;
                _contexto.Entry(Persona).State = EntityState.Modified;
                _contexto.SaveChanges();

            }

            //Agregamos el prestamo 
            _contexto.Prestamos.Add(Prestamo);
            int cantidad = _contexto.SaveChanges();

            return cantidad > 0;
        }

        private bool Modificar(Prestamo Prestamo)
        {

            var Persona = _contexto.Personas.Find(Prestamo.PersonaId);
            var anterior = _contexto.Prestamos.Find(Prestamo.PrestamoId);
            if (Persona != null)
            {

                Prestamo.Balance -= anterior.Monto;
                Prestamo.Balance += Prestamo.Monto;

                Persona.Balance -= anterior.Balance;
                Persona.Balance += Prestamo.Balance;

                _contexto.Entry(Persona).State = EntityState.Modified;
                _contexto.SaveChanges();

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
            //modificamos  a la persona
            var Persona = _contexto.Personas.Find(Prestamo.PersonaId);
            if (Persona != null)
            {
                Persona.Balance -= Prestamo.Balance;

                _contexto.Entry(Persona).State = EntityState.Modified;
                _contexto.SaveChanges();

            }

            //eliminamos el prestamo
            _contexto.Remove(Prestamo);
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM Prestamos WHERE PrestamoId={Prestamo.PrestamoId};");
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM PagoDetalles WHERE PrestamoId={Prestamo.PrestamoId};");
            _contexto.Entry(Prestamo).State = EntityState.Detached;
            return _contexto.SaveChanges() > 0;
        }

        public Prestamo? Buscar(int PrestamoID)
        {
            return _contexto.Prestamos.Find(PrestamoID);

        }
        public List<Prestamo> GetList()
        {
            return _contexto.Prestamos.Where(o => o.PrestamoId != 0).AsNoTracking().ToList();
        }
    }
}