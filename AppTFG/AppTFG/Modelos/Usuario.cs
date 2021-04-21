using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTFG.Modelos
{
    public class Usuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string FirebaseToken { get; set; }
    }
}
