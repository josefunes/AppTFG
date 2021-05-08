using Acr.UserDialogs;
using AppTFG.FormsVideoLibrary;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            if (video.Id == 0)
            {
                this.ToolbarItems.RemoveAt(1);
                Title = "Nuevo vídeo";
                videoPlayer.IsVisible = false;
                videoPlayer.IsEnabled = false;
            }
            else
            {
                Title = Video.Nombre;
                videoPlayer.Source = new FileVideoSource
                {
                    File = video.Videoclip
                };
            }
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Guardando vídeo...");
            }
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void Loading1(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Eliminando vídeo...");
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

            var video = await ServicioMultimedia.SeleccionarVideo();
            if (video != null) 
            {
                string videoPath = video.Path;
                if (!string.IsNullOrWhiteSpace(videoPath))
                {
                    if(videoPlayer.IsVisible == false)
                    {
                        videoPlayer.IsVisible = true;
                    }
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
                UserDialogs.Instance.Alert(Constantes.TitleVideoRequired, "Advertencia", "OK");
                Loading(false);
                return;
            }
            if (video.Id > 0)
            {
                if (video.Stream == null)
                {
                    await FirebaseHelper.ActualizarVideo(video.Id, video.Nombre, video.Videoclip, video.IdPueblo);
                }
                else
                {
                    await FirebaseHelper.ActualizarVideo(video.Id, video.Nombre, video.Videoclip = await FirebaseHelper.SubirVideo(video.Stream, video.Nombre), video.IdPueblo);
                }
            }
            else
            {
                await FirebaseHelper.InsertarVideo(video.Id = Constantes.GenerarId(), video.Nombre, video.Videoclip = await FirebaseHelper.SubirVideo(video.Stream, video.Nombre), video.IdPueblo);
            }
            Loading(false);
            UserDialogs.Instance.Alert("Registro del vídeo realizado correctamente", "Correcto", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este vídeo?", "Si", "No"))
            {
                Loading1(true);
                await FirebaseHelper.EliminarVideo(Video.Id);
                await FirebaseHelper.BorrarVideo(Video.Nombre);
                Loading1(false);
                UserDialogs.Instance.Alert("Vídeo eliminado correctamente", "Correcto", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}