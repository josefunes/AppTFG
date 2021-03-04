using AppTFG.FormsVideoLibrary;
using AppTFG.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaReproduccionVideo : ContentPage
    {
        Video Video;
        public PaginaReproduccionVideo(Video video)
        {
            InitializeComponent();
            Video = video;
            BindingContext = video;
            //Con esto le envío al reproductor el vídeo que he seleccionado en la pantalla anterior
            videoPlayer.Source = new FileVideoSource
            {
                File = video.Videoclip
            };
            Title = Video.Nombre;
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
    }
}