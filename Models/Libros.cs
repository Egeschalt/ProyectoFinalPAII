using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalPAII.Models
{
    public class Libros
    {
        public int LibrosId { get; set; }
        public string LibrosName { get; set; }
        public int AutorId { get; set; }
        public int EditorialId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaLanzamiento { get; set; }

        public Autor Autor { get; set; }
        public Editorial Editorial { get; set; }    
    }
}
