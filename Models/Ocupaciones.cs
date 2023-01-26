using System.ComponentModel.DataAnnotations;

namespace GestionPrestamosPersonales2023
{
    public class Ocupaciones
    {
        [Key]
        public  int OcupacionId{get;set;}
        public string? Descripcion{get;set;}
        public double Salario{get;set;}

    }
}