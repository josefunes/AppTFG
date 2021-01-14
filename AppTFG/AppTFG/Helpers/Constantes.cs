using AppTFG.Modelos;
using System.Collections.ObjectModel;

namespace AppTFG.Helpers
{
    public class Constantes
    {
        public Constantes() { }
        public static readonly string NombreBD = "BD_SistemaRural.db";
        public static ObservableCollection<Imagenes> GalleryCollection;
        public const string AppTitle = "AppTFG";//Cambiar cuando haya un nombre final
        public const string TitleRequired = "Por favor, introduce un título para la imagen.";
        public const string Ok = "Ok";
        public const string AppUploadTitle = "Subir imagen";
        public const string PermissionDenied = "Permiso no otorgado para fotos";
    }
}
