using SQLite;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xamarin.Forms.Maps;

namespace AppTFG.Modelos
{
    public class Ruta
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenPrincipal { get; set; }
        public Map Mapa { get; set; }
        public Video VideoUrl { get; set; }
        [ForeignKey("FK_NombrePueblo")]
        public int IdPueblo { get; set; }
    }
}
