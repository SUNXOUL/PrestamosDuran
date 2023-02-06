using System.ComponentModel.DataAnnotations;

namespace GestionPrestamosPersonales2023
{
    public class Pago
    {
        [Key]
        public int PagoId{get;set;}
        public DateOnly Fecha{get;set;}
        public int PersonaId{get;set;}
        public string? Concepto{get;set;}
        public int Monto{get;set;}
    }
}