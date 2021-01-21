using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Xamarin.Forms;

namespace AppTFG.Modelos
{
    public class Pueblo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Introduccion { get; set; }
        public string Descripcion { get; set; }
        public string ImagenPrincipal { get; set; }
        public List<Foto> Fotos { get; set; }
        public List<Video> Videos { get; set; }
        public List<Ruta> Rutas { get; set; }
        public List<Actividad> Actividades { get; set; }
    }
}

