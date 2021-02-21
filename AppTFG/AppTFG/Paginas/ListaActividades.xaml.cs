using AppTFG.Modelos;
using AppTFG.Helpers;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaActividades : ContentPage
    {
        public ListaActividades()
        {
            InitializeComponent();
            Title = "Lista de Actividades";
        }

        public ListaActividades(Pueblo pueblo)
        {
            InitializeComponent();

            this.BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            //var bd = new ServicioBaseDatos<Actividad>();
            //lsvActividades.ItemsSource = await bd.ObtenerTabla();
            lsvActividades.ItemsSource = await FirebaseHelper.ObtenerTodasActividades();
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void LsvActividades_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Actividad)e.SelectedItem;
                await Navigation.PushAsync(new PaginaActividad(dato));
                lsvActividades.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}