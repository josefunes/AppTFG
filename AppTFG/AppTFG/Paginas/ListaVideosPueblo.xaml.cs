using AppTFG.FormsVideoLibrary;
using System;

using Xamarin.Forms;

namespace AppTFG.Paginas
{
    public partial class ListaVideosPueblo : ContentPage
    {
        public ListaVideosPueblo()
        {
            InitializeComponent();
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