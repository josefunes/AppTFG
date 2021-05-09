using Acr.UserDialogs;
using AppTFG.Helpers;
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
    public partial class PaginaVistaListaVideos : ContentPage
    {
        public Video Video;
        public PaginaVistaListaVideos(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Videos de " + pueblo.Nombre;
            BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvVideosPueblo.ItemsSource = null;
                lsvVideosPueblo.ItemsSource = await FirebaseHelper.ObtenerTodosVideosPueblo(pueblo.Id);
            }
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                indicator.HeightRequest = 30;
            }
            else
            {
                indicator.HeightRequest = 0;
            }
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        public async void LsvVideosPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Video)e.SelectedItem;
                await Navigation.PushAsync(new PaginaReproduccionVideo(dato));
                lsvVideosPueblo.SelectedItem = null;
            }
            catch (Exception)
            {
                //UserDialogs.Instance.Alert("Se ha producido un error", "", "Ok");
            }
        }
    }
}