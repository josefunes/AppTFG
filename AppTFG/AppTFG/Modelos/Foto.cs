using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTFG.Modelos
{
    public class Foto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }

        [ForeignKey("FK_NombrePueblo")]
        public int IdPueblo { get; set; }
        public Pueblo Pueblo { get; set; }
    }
}
