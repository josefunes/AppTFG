﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTFG.Modelos
{
    public class Posicion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Posicion() { }
        public Posicion(int id, double x, double y)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }
}

