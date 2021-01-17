using AppTFG.FormsVideoLibrary;
using AppTFG.Modelos;
using System;

using Xamarin.Forms;

namespace AppTFG.Paginas
{
    public partial class ListaVideosPueblo : ContentPage
    {
        public ListaVideosPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Videos de " + pueblo.Nombre;
            this.BindingContext = pueblo;
        }

        async void OnShowVideoLibraryClicked(object sender, EventArgs args)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;

            string filename = await DependencyService.Get<IVideoPicker>().GetVideoFileAsync();

            if (!string.IsNullOrWhiteSpace(filename))
            {
                videoPlayer.Source = new FileVideoSource
                {
                    File = filename
                };
            }

            btn.IsEnabled = true;
        }
    }
}