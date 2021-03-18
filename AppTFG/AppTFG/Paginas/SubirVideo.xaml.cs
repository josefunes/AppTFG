using Acr.UserDialogs;
using AppTFG.FormsVideoLibrary;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubirVideo : ContentPage
    {
        Video Video;
        public SubirVideo(Video video)
        {
            InitializeComponent();
            Video = video;
            BindingContext = video;
            //Con esto le envío al reproductor el vídeo que he seleccionado en la pantalla anterior
            videoPlayer.Source = new FileVideoSource
            {
                File = video.Videoclip
            };
            if (video.Id == 0)
            {
                this.ToolbarItems.RemoveAt(1);
                Title = "Nuevo video";
            }
            else
            {
                Title = Video.Nombre;
            }
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
            }
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async void BtnVideo_Clicked(object sender, EventArgs args)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;

            //string videoPath = await DependencyService.Get<IVideoPicker>().GetVideoFileAsync();
            var video = await ServicioMultimedia.SeleccionarVideo();
            if (video != null) 
            {
                string videoPath = video.Path;
                if (!string.IsNullOrWhiteSpace(videoPath))
                {

                    videoPlayer.Source = new FileVideoSource
                    {
                        File = videoPath
                    };
                    Video.Videoclip = videoPath;
                    Video.Stream = video.GetStream();
                }
                btn.IsEnabled = true;
            }
            else { }
        }

        void OnPlayPauseButtonClicked(object sender, EventArgs args)
        {
            if (videoPlayer.Status == VideoStatus.Playing)
            {
                videoPlayer.Pause();
            }
            else if (videoPlayer.Status == VideoStatus.Paused)
            {
                videoPlayer.Play();
            }
        }

        void OnStopButtonClicked(object sender, EventArgs args)
        {
            videoPlayer.Stop();
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var video = (Video)BindingContext;
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                UserDialogs.Instance.Alert("Advertencia", Constantes.TitleVideoRequired, "OK");
                return;
            }
            if (video.Id > 0)
            {
                await FirebaseHelper.ActualizarVideo(video.Id, video.Nombre, video.Videoclip);
            }
            else
            {
                //var videoclip = await FirebaseHelper.SubirVideo(video.Stream, video.Nombre);
                await FirebaseHelper.InsertarVideo(video.Id = Constantes.GenerarId(), video.Nombre, video.Videoclip = await FirebaseHelper.SubirVideo(video.Stream, video.Nombre), video.IdPueblo);
            }
            Loading(false);
            UserDialogs.Instance.Alert("Correcto", "Registro del vídeo realizado correctamente", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este vídeo?", "Si", "No"))
            {
                Loading(true);
                await FirebaseHelper.EliminarVideo(Video.Id);
                await FirebaseHelper.BorrarVideo(Video.Nombre);
                Loading(false);
                UserDialogs.Instance.Alert("Correcto", "Vídeo eliminado correctamente", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}