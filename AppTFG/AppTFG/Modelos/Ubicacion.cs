using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTFG.Modelos
{
    public class Ubicacion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }

        public Ubicacion() { }

        public Ubicacion(int id, string name, double x, double y)
        {
            Id = id;
            Nombre = name;
            Latitud = x;
            Longitud = y;
        }
    }
}
