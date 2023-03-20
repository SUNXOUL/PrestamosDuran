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
            var persona = _contexto.Personas.Find(Pago.PersonaId);

            //agregamos el monto total
            IEnumerable<Double> montos = from monto in Pago.Detalles select monto.ValorPagado;
            Pago.Monto = montos.Sum();

            //agregamos los nuevos
            foreach (var item in Pago.Detalles)
            {
                var prestamo = _contexto.Prestamos.Find(item.PrestamoId);
                prestamo.Balance -= item.ValorPagado;
                _contexto.Prestamos.Update(prestamo);
            }

            //reducimos el monto del balance de la persona
            persona.Balance -= Pago.Monto;
            _contexto.Personas.Update(persona);

            _contexto.Pagos.Add(Pago);
            int cantidad = _contexto.SaveChanges();

            return cantidad > 0;
        }

        public bool Modificar(Pago pago)
        {

            //pago anterior
            var anterior = _contexto.Pagos.Where(o => o.PagoId == pago.PagoId).Include(o => o.Detalles).AsNoTracking().SingleOrDefault();

            //persona
            var persona = _contexto.Personas.Find(pago.PersonaId);


            //eliminamos los montos anteriores al prestamo
            foreach (var item in anterior.Detalles)
            {
                var prestamo = _contexto.Prestamos.Find(item.PrestamoId);
                prestamo.Balance += item.ValorPagado;
                _contexto.Prestamos.Update(prestamo);
            }

            //agregamos los nuevos al prestamo
            foreach (var item in pago.Detalles)
            {
                var prestamo = _contexto.Prestamos.Find(item.PrestamoId);
                prestamo.Balance -= item.ValorPagado;
                _contexto.Prestamos.Update(prestamo);
            }

            //eliminamos y agregamos los nuevos detalles
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM PagoDetalles WHERE PagoId={pago.PagoId};");
            foreach (var item in pago.Detalles)
            {
                _contexto.Entry(item).State = EntityState.Added;
            }

            //actualizamos el nuevo valor del monto del pago
            IEnumerable<Double> montos = from monto in pago.Detalles select monto.ValorPagado;
            pago.Monto = montos.Sum();


            //actualizamos el pago en la persona
            persona.Balance += anterior.Monto;
            persona.Balance -= pago.Monto;
            _contexto.Personas.Update(persona);

            //actualizamos el pago
            _contexto.Entry(pago).State = EntityState.Modified;
            var guardo = _contexto.SaveChanges() > 0;
            _contexto.Entry(pago).State = EntityState.Detached;
            return guardo;
        }


        public bool Eliminar(Pago Pago)
        {
            //persona
            var persona = _contexto.Personas.Find(Pago.PersonaId);

            //actualizamos el pago en la persona
            persona.Balance += Pago.Monto;
            _contexto.Personas.Update(persona);

            //actualizamos el prestamo
            foreach (var item in Pago.Detalles)
            {
                var prestamo = _contexto.Prestamos.Find(item.PrestamoId);
                prestamo.Balance += item.ValorPagado;
                _contexto.Prestamos.Update(prestamo);
                _contexto.SaveChanges();
            }

            //eliminamos el pago
            _contexto.Remove(Pago);
            bool cambios = _contexto.SaveChanges() > 0;
            _contexto.Entry(Pago).State = EntityState.Detached;
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM Pagos WHERE PagoId={Pago.PagoId};");
            _contexto.Database.ExecuteSqlRaw($"DELETE FROM PagoDetalles WHERE PagoId={Pago.PagoId};");
            return cambios;
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
            return _contexto.Pagos.Where(o => o.PagoId != 0).AsNoTracking().ToList();
        }

    }
}