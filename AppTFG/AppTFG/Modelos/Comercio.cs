using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace AppTFG.Modelos
{
    public class Comercio
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenPrincipal { get; set; }
        public string Direccion { get; set; }
        public string Horario { get; set; }
        public string Contacto { get; set; }
        public Ubicacion Ubicacion { get; set; }
        public Stream Stream { get; set; }
        public Video VideoUrl { get; set; }
        [ForeignKey("FK_IdPueblo")]
        public int IdPueblo { get; set; }
    }
}
