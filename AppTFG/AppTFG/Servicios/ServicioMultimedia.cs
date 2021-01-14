using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AppTFG.Servicios
{
    public static class ServicioMultimedia
    {
        public static async Task<MediaFile> SeleccionarImagen()
        {
            await CrossMedia.Current.Initialize();
            MediaFile file = await CrossMedia.Current.PickPhotoAsync();
            return file;
        }

        public static async Task<MediaFile> SeleccionarVideo()
        {
            await CrossMedia.Current.Initialize();
            MediaFile file = await CrossMedia.Current.PickVideoAsync();
            return file;
        }
    }
}