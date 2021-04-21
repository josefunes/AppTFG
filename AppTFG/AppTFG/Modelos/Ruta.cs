using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Xamarin.Forms;

namespace AppTFG.Modelos
{
    public class Ruta
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenPrincipal { get; set; }
        public Stream Stream { get; set; }
        public List<Posicion> Camino { get; set; }
        public List<Ubicacion> Ubicaciones { get; set; }
        public List<Audio> Audios { get; set; }
        public Video VideoUrl { get; set; }
        [ForeignKey("FK_IdPueblo")]
        public int IdPueblo { get; set; }
    }
}
