using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalPAII.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public int DNI { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaSuscripcion { get; set; }

        public ICollection<Prestamos> Prestamos { get; set; }
    }
}
