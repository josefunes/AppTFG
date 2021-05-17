using System;
using SQLite;

namespace AppTFG.Modelos
{
    public class Saltar
    {
        [PrimaryKey]
        public int ID { get; set; }
        public bool Salta { get; set; }
    }
}
