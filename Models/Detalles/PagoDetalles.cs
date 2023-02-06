using System.ComponentModel.DataAnnotations;
namespace GestionPrestamosPersonales2023
{
    public class PagoDetalles
    {
                [Key]
        public int Id{get;set;}
        public int PagoId{get;set;}
        public int PrestamoId{get;set;}
        public double? ValorPagado{get;set;}
    }
}