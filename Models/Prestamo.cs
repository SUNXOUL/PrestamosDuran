using System.ComponentModel.DataAnnotations;
namespace GestionPrestamosPersonales2023
{
    public class Prestamo
    {
        [Key]

        public int PrestamoId { get; set; }

        public DateOnly F_Inicio { get; set; }

        public DateOnly F_Vencimiento { get; set; }

        public int PersonaId { get; set; }

        public String? Concepto { get; set; }

        public int Monto { get; set; }

    }
}