using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalPAII.Models
{
    public class Devoluciones
    {
        public int DevolucionesId { get; set; }
        public int PrestamoId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDevolucion { get; set; }

        public Prestamos Prestamo { get; set; }
    }
}
