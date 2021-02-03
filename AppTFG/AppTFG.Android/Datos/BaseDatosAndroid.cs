using System;
using System.IO;
using AppTFG.Datos;
using AppTFG.Droid.Datos;
using AppTFG.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseDatosAndroid))]
namespace AppTFG.Droid.Datos
{
    public class BaseDatosAndroid : IBaseDatos
    {
        
        public string GetDatabasePath()
        {
            var filename = Constantes.NombreBD;
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, filename);
            //var connection = new SQLiteConnection(path);
            return path;
        }
    }
}