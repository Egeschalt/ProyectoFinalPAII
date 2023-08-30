namespace ProyectoFinalPAII.Models
{
    public class Editorial
    {
        public int EditorialId { get; set; }
        public string EditorialName { get; set; }
        public ICollection<Libros> Libros { get; set; }

    }
}
