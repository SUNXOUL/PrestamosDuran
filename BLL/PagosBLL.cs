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
        return _contexto.Pagos.Any(o => o.PagoId == PagoId);
    }

    private bool Insertar(Pago Pago)
    {
        _contexto.Pagos.Add(Pago);
        int cantidad = _contexto.SaveChanges();
        foreach(var detalle in Pago.Detalles)
        {
            System.Console.WriteLine(Pago.Monto);
            Pago.Monto+=detalle.ValorPagado;
        }
        System.Console.WriteLine(Pago.Monto);
        
            //Agregamos el monto del balance de la persona
            var persona =_contexto.Personas.Find(Pago.PersonaId);
            persona.Balance-=Pago.Monto;
            _contexto.Entry(persona).State = EntityState.Modified;
            _contexto.SaveChanges();
            _contexto.Entry(persona).State = EntityState.Detached;

        return cantidad > 0;
    }

    public bool Modificar(Pago pago)
    {

            var anterior = _contexto.Pagos
            .Where(c => c.PagoId == pago.PagoId)
            .Include(c => c.Detalles)
            .AsNoTracking()
            .SingleOrDefault();

            //eliminamos los antiguos montos detalles del pago
            foreach (var item in anterior.Detalles)
            {
                pago.Monto -= item.ValorPagado;
            }


            //Modificamos el monto del balance de la persona
            if (anterior.Monto != pago.Monto)
            {
                var persona = _contexto.Personas.Find(pago.PersonaId);
                persona.Balance -= anterior.Monto;
                persona.Balance += pago.Monto;
                _contexto.Entry(persona).State = EntityState.Modified;
                _contexto.SaveChanges();
                _contexto.Entry(persona).State = EntityState.Detached;
            }

            //agregamos los nuevos montos al pago
            foreach (var item in pago.Detalles)
            {
                pago.Monto += item.ValorPagado;

                _contexto.Entry(item).State = EntityState.Added;
            }

            //actualizamos el pago
            _contexto.Entry(pago).State = EntityState.Modified;
            var guardo = _contexto.SaveChanges() > 0;
            _contexto.Entry(pago).State = EntityState.Detached;
            return guardo;
    }
        public bool Eliminar(Pago Pago)
        {

            //eliminamos el monto del balance de la persona
            var persona = _contexto.Personas.Find(Pago.PersonaId);
            persona.Balance+=Pago.Monto;
            _contexto.Entry(persona).State = EntityState.Modified;
            _contexto.SaveChanges();
            _contexto.Entry(persona).State = EntityState.Detached;


            //eliminamos el pago
            _contexto.Entry(Pago).State=EntityState.Deleted;
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM Pagos WHERE PagoId={Pago.PagoId};");
            _contexto.Entry(Pago).State = EntityState.Detached;
            return _contexto.SaveChanges()>0;
        }   

        public Pago? Buscar(int PagoID)
        {
            return _contexto.Pagos
                .Include(c => c.Detalles)
                .Where(c => c.PagoId == PagoID)
                .AsNoTracking()
                .SingleOrDefault();
        }

        public List<Pago> GetList()
        {
            return _contexto.Pagos.Where(o=>o.PagoId!=0).AsNoTracking().ToList();
        }
    
    }
}