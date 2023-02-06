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
        return _contexto.Persona.Any(o => o.PersonaId == PersonaId);
    }

    private bool Insertar(Persona Persona)
    {
        _contexto.Persona.Add(Persona);
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
        return _contexto.Persona.ToList();
    }

        public bool Eliminar(Persona Persona)
        {
            _contexto.Entry(Persona).State=EntityState.Deleted;
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM Persona WHERE PersonaId={Persona.PersonaId};");
            _contexto.Entry(Persona).State = EntityState.Detached;
            return _contexto.SaveChanges()>0;
        }   

        public Persona? Buscar(int PersonaID)
        {
            return _contexto.Persona
                    .Where(o => o.PersonaId==PersonaID ).AsNoTracking().SingleOrDefault();
                    
        }
        public List<Persona> GetList()
        {
            return _contexto.Persona.AsNoTracking().ToList();
        }
    }
}