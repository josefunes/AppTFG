using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTFG.Modelos
{
    public class Usuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, AutoIncrement]
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        [ForeignKey("FK_NombrePueblo")]
        public int IdPueblo { get; set; }
        //public Pueblo Pueblo { get; set; }
    }
}
