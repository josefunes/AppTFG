using AppTFG.Modelos;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppTFG.Helpers
{
    public static class Constantes
    {
        public static readonly string NombreBD = "BD_SistemaRural.db3";
        public const string AppTitle = "AppTFG";//Cambiar cuando haya un nombre final
        public const string TitleImagenRequired = "Por favor, introduce un título para la imagen.";
        public const string TitleVideoRequired = "Por favor, introduce un título para el vídeo.";
        public const string TitlePuebloRequired = "Por favor, introduce un nombre para el pueblo.";
        public const string TitleRutaRequired = "Por favor, introduce un título para la ruta.";
        public const string TitleActividadRequired = "Por favor, introduce un título para la actividad.";
        public const string TitleComercioRequired = "Por favor, introduce un título para el comercio.";
        public const string TitleAlojamientoRequired = "Por favor, introduce un título para el alojamiento.";
        public const string Codigo = "";
        //public const string InsertImageRequired = "Por favor, añade una imagen.";
        public static int GenerarId()
        {
            var guid = Guid.NewGuid();
            var numeros = new string(guid.ToString().Where(char.IsDigit).ToArray());
            return int.Parse(numeros.Substring(0, 4));
        }
        public static string Cifrar(string cadena)
        {
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(cadena);
            string result = Convert.ToBase64String(encryted);
            return result;
        }

        /// Esta función descifra la cadena que le envíamos en el parámentro de entrada.
        public static string Descifrar(this string cadena)
        {
            byte[] decryted = Convert.FromBase64String(cadena);
            string result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
    }
}
