﻿namespace ProyectoFinalPAII.Models
{
    public class Autor
    {
        public int AutorId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Nacionalidad { get; set; }
        public ICollection<Libros> Libros { get; set; }
    }
}
