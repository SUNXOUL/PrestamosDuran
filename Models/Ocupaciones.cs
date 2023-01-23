using System.ComponentModel.DataAnnotations;

namespace GestionPrestamosPersonales2023
{
    public class Ocupaciones
    {
        [Key]
        public  int OcupacionId{get;set;}

        [Required(ErrorMessage ="La Descripcion es Requerida")]
        public string? Descripcion{get;set;}

    }
}