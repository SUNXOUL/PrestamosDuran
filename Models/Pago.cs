using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GestionPrestamosPersonales2023
{
    public class Pago
    {
        [Key]
        public int PagoId {get;set;}
        public DateTime Fecha{get;set;}
        public int PersonaId{get;set;}
        public string? Concepto{get;set;}
        public double Monto{get;set;}

        [ForeignKey("PagoId ")]
        public  List<PagoDetalles> Detalles {get;set;} = new List<PagoDetalles>();

        public Pago()
        {
            this.Fecha=DateTime.Now;
            this.Monto=0;
            this.Detalles = new List<PagoDetalles>();
        }
    }

        public class PagoDetalles
    {
                [Key]
        public int Id{get;set;} 
        public int PagoId { get; set; }
        public int PrestamoId{get;set;}
        public double ValorPagado{get;set;}

    }
}