using AppTFG.Modelos;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaActividadesPueblo : ContentPage
    {
        public ListaActividadesPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Actividades de " + pueblo.Nombre;
            BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var bd = new ServicioBaseDatos<Actividad>();
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvActividadesPueblo.ItemsSource = null;
                lsvActividadesPueblo.ItemsSource = await bd.ObtenerTabla();
                lsvActividadesPueblo.ItemsSource = pueblo.Actividades;
            }
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void LsvActividadesPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Actividad)e.SelectedItem;
                await Navigation.PushAsync(new PaginaActividad(dato));
                lsvActividadesPueblo.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new PaginaActividad(new Actividad() { Pueblo = pueblo }));
        }
    }
}