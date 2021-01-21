using AppTFG.FormsVideoLibrary;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaVideosPueblo : ContentPage
    {
        Video Video;
        public ListaVideosPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Videos de " + pueblo.Nombre;
            this.BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var bd = new ServicioBaseDatos<Video>();
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvVideosPueblo.ItemsSource = null;
                lsvVideosPueblo.ItemsSource = await bd.ObtenerTabla();
                lsvVideosPueblo.ItemsSource = pueblo.Videos;
            }
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private void LsvVideosPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var bd = new ServicioBaseDatos<Video>().ObtenerTabla();
                var dato = (Video)e.SelectedItem;
                if (!string.IsNullOrWhiteSpace(dato.Videoclip))
                {

                    videoPlayer.Source = new FileVideoSource
                    {
                        File = dato.Videoclip
                    };
                    Video.Videoclip = dato.Videoclip;
                }
                //lsvVideosPueblo.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new SubirVideo(new Video() { Pueblo = pueblo }));
        }
    }
}