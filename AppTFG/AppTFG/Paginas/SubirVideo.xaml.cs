using AppTFG.FormsVideoLibrary;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubirVideo : ContentPage
    {
        ServicioBaseDatos<Video> bd;
        Video Video;
        public SubirVideo(Video video)
        {
            InitializeComponent();
            Video = video;
            BindingContext = video;
            bd = new ServicioBaseDatos<Video>();

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
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        //private async void BtnVideo_Clicked(object sender, EventArgs e)
        //{
        //    var video = await ServicioMultimedia.SeleccionarVideo();
        //    Video.Video = video.Path;
        //    vdoVideo.VideoPlayer = VideoSource.FromFile(video.Path);
        //}

        async void BtnVideo_Clicked(object sender, EventArgs args)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;

            string videoPath = await DependencyService.Get<IVideoPicker>().GetVideoFileAsync();

            if (!string.IsNullOrWhiteSpace(videoPath))
            {

                videoPlayer.Source = new FileVideoSource
                {
                    File = videoPath
                };
                Video.Videoclip = videoPath;
            }
            btn.IsEnabled = true;
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
                await DisplayAlert("Advertencia", Constantes.TitleVideoRequired, "OK");
                return;
            }
            if (video.Id > 0)
                await bd.Actualizar(video);
            else
                await bd.Agregar(video);

            Loading(false);
            await DisplayAlert("Correcto", "Registro del vídeo realizado correctamente", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este vídeo?", "Si", "No"))
            {
                Loading(true);
                await bd.Eliminar(((Video)BindingContext).Id);
                Loading(false);
                await DisplayAlert("Correcto", "Vídeo eliminado correctamente", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}