using AppTFG.Modelos;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaRutasPueblo : ContentPage
    {
        public ListaRutasPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Rutas de " + pueblo.Nombre;
            BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var bd = new ServicioBaseDatos<Ruta>();
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvRutasPueblo.ItemsSource = null;
                lsvRutasPueblo.ItemsSource = await bd.ObtenerTabla();
                lsvRutasPueblo.ItemsSource = pueblo.Rutas;
            }
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void LsvRutasPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Ruta)e.SelectedItem;
                await Navigation.PushAsync(new PaginaRuta(dato));
                lsvRutasPueblo.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new PaginaRuta(new Ruta() { Pueblo = pueblo }));
        }
    }
}