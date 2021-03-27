using AppTFG.FormsVideoLibrary;
using AppTFG.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        Video Video;
        public Page1(Video video)
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
    }
}