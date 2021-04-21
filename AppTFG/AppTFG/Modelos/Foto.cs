using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace AppTFG.Modelos
{
    public class Foto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public Stream Stream { get; set; }

        [ForeignKey("FK_IdPueblo")]
        public int IdPueblo { get; set; }
    }
}
