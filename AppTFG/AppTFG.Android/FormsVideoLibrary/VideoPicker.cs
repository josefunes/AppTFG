using System.Threading.Tasks;
using Android.Content;
using AppTFG.FormsVideoLibrary;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppTFG.Droid.VideoPicker))]

namespace AppTFG.Droid
{
    public class VideoPicker : IVideoPicker
    {
        public Task<string> GetVideoFileAsync()
        {
            // Define el Intent para obtener los vídeos
            Intent intent = new Intent();
            intent.SetType("video/*");
            intent.SetAction(Intent.ActionGetContent);

            // Obtiene la instancia del MainActivity
            MainActivity activity = MainActivity.Current;

            // Inicia la actividad de seleccionar el vídeo
            activity.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Video"),
                MainActivity.PickImageId);

            // Guarda el objeto TaskCompletionSource como una propiedad del MainActivity
            activity.PickImageTaskCompletionSource = new TaskCompletionSource<string>();

            // Devuelve el objeto Task
            return activity.PickImageTaskCompletionSource.Task;
        }
    }
}