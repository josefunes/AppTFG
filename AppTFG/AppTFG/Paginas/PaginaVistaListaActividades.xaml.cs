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
    public partial class PaginaVistaListaActividades : ContentPage
    {
        public PaginaVistaListaActividades(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Actividades de " + pueblo.Nombre;
            BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvActividadesPueblo.ItemsSource = null;
                lsvActividadesPueblo.ItemsSource = await FirebaseHelper.ObtenerTodasActividadesPueblo(pueblo.Id);
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
                await Navigation.PushAsync(new PaginaVistaActividad(dato));
                lsvActividadesPueblo.SelectedItem = null;
            }
            catch (Exception)
            {
            }
        }
    }
}