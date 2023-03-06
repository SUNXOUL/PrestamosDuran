using System.ComponentModel.DataAnnotations;

namespace GestionPrestamosPersonales2023
{
    public class Ocupacion
    {
        [Key]
        public  int OcupacionId{get;set;}
        public string? Descripcion{get;set;}
        public double Salario{get;set;}

    }
}