﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace AppTFG.Modelos
{
    public class Actividad
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenPrincipal { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public List<int> Valoraciones { get; set; }
        public Stream Stream { get; set; }
        public Video VideoUrl { get; set; }
        [ForeignKey("FK_IdPueblo")]
        public int IdPueblo { get; set; }
    }
}
