using System.ComponentModel.DataAnnotations;
namespace GestionPrestamosPersonales2023
{
    public class Persona
    {
        [Key]
        public int PersonaId { get; set; }

        public String? Nombre { get; set; }
        public String? Telefono { get; set; }
        public String? Celular { get; set; }
        public double Balance { get; set; }
        public String? Email { get; set; }
        public String? Direccion { get; set; }

        public DateOnly? F_Nacimiento { get; set; }

        public int OcupacionId { get; set; }
    }

}