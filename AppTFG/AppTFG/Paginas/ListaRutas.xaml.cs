using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaRutas : ContentPage
    {
        public ListaRutas()
        {
            InitializeComponent();
            Title = "Lista de Rutas";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            //var bd = new ServicioBaseDatos<Ruta>();
            //lsvRutas.ItemsSource = await bd.ObtenerTabla();
            lsvRutas.ItemsSource = await FirebaseHelper.ObtenerTodasRutas();
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

        private async void LsvRutas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Ruta)e.SelectedItem;
                await Navigation.PushAsync(new PaginaRuta(dato));
                lsvRutas.SelectedItem = null;
            }
            catch (Exception)
            {
            }
        }
    }
}