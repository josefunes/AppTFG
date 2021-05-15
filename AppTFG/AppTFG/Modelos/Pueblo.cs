using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace AppTFG.Modelos
{
    public class Pueblo : IComparable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenPrincipal { get; set; }
        public Stream Stream { get; set; }
        public List<Foto> Fotos { get; set; }
        public List<Video> Videos { get; set; }
        public List<Ruta> Rutas { get; set; }
        public List<Actividad> Actividades { get; set; }

        public int CompareTo(object obj)
        {
            return Nombre.CompareTo(obj);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Pueblo objAsPueblo = obj as Pueblo;
            if (objAsPueblo == null) return false;
            else return Equals(objAsPueblo);
        }
        public int SortByNameAscending(string name1, string name2)
        {
            return name1.CompareTo(name2);
        }

        // Default comparer for Pueblo type.
        public int CompareTo(Pueblo comparePueblo)
        {
            // A null value means that this object is greater.
            if (comparePueblo == null)
                return 1;

            else
                return this.Id.CompareTo(comparePueblo.Id);
        }
    }
}

