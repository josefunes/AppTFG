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
            return Path.Combine(
                System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal),
                Constantes.NombreBD);
        }
    }
}