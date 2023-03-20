using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
namespace GestionPrestamosPersonales2023
{
    public class PersonasBLL
    {
        private Contexto _contexto;
        public PersonasBLL(Contexto contexto)
        {
            _contexto = contexto;
        }

        public bool Guardar(Persona Persona)
        {
            if (!Existe(Persona.PersonaId))
                return Insertar(Persona);
            else
                return Modificar(Persona);
        }

        public bool Existe(int PersonaId)
        {
            return _contexto.Personas.Any(o => o.PersonaId == PersonaId);
        }

        private bool Insertar(Persona Persona)
        {
            _contexto.Personas.Add(Persona);
            int cantidad = _contexto.SaveChanges();
            return cantidad > 0;
        }

        public bool Modificar(Persona Persona)
        {
            _contexto.Entry(Persona).State = EntityState.Modified;
            int cantidad = _contexto.SaveChanges();
            _contexto.Entry(Persona).State = EntityState.Detached;
            return cantidad > 0;
        }

        public List<Persona> GetPersonas()
        {
            return _contexto.Personas.ToList();
        }

        public bool Eliminar(Persona Persona)
        {
            bool cambios = false;
            if (Persona != null)
            {
                _contexto.Remove(Persona);
                cambios = _contexto.SaveChanges() > 0;
                _contexto.Entry(Persona).State = EntityState.Detached;
                _contexto.Database.ExecuteSqlRaw($"DELETE FROM Pagos WHERE PersonaId={Persona.PersonaId};");
                _contexto.Database.ExecuteSqlRaw($"DELETE FROM Prestamos WHERE PersonaId={Persona.PersonaId};");
                _contexto.Database.ExecuteSqlRaw($"DELETE FROM Personas WHERE PersonaId={Persona.PersonaId};");
            }

            return cambios;
        }

        public Persona? Buscar(int PersonaID)
        {
            return _contexto.Personas
                    .Where(o => o.PersonaId == PersonaID).AsNoTracking().SingleOrDefault();

        }
        public List<Persona> GetList()
        {
            return _contexto.Personas.AsNoTracking().ToList();
        }
    }
}