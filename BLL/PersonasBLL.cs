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
        return cantidad > 0;
    }
    
    public List<Persona> GetPersonas()
    {
        return _contexto.Personas.ToList();
    }

        public bool Eliminar(Persona Persona)
        {
            Console.WriteLine("eliminado");
            _contexto.Entry(Persona).State=EntityState.Deleted;
            return _contexto.SaveChanges()>0;
        }   

        public Persona? Buscar(int PersonaID)
        {
            return _contexto.Personas
                    .Where(o => o.PersonaId==PersonaID ).AsNoTracking().SingleOrDefault();
                    
        }
        public List<Persona> GetList()
        {
            return _contexto.Personas.AsNoTracking().ToList();
        }
    }
}