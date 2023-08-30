using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalPAII.Models
{
    public class Prestamos
    {
        public int PrestamosId { get; set; }
        public int ClienteId { get; set; }
        public int LibroId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaPrestamo { get; set; }
        public string Estado { get; set; }

        public Cliente Cliente { get; set; }
        public Libros Libro { get; set; }
    }
}
