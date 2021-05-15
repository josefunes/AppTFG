using AppTFG.Modelos;
using AppTFG.Helpers;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaActividades : ContentPage
    {
        public static List<Actividad> Actividades { get; set; }
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
            Actividades = await FirebaseHelper.ObtenerTodasActividades();
            lsvActividades.ItemsSource = Actividades;
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
                await Navigation.PushAsync(new PaginaVistaActividad(dato));
                lsvActividades.SelectedItem = null;
            }
            catch (Exception)
            {
                //UserDialogs.Instance.Alert("Se ha producido un error", "", "Ok");
            }
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                lsvActividades.ItemsSource = Actividades;
            else
                lsvActividades.ItemsSource = GetSearchResults(e.NewTextValue);
        }

        public List<Actividad> GetSearchResults(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            var listaActividades = (List<Actividad>)lsvActividades.ItemsSource;
            return listaActividades.Where(f => f.Nombre.ToLowerInvariant().Contains(normalizedQuery)).ToList();
        }

        void OrdenAlfabeticoAscendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                if (searchBar.Text == null)
                    searchBar.Text = "";
                lsvActividades.ItemsSource = GetSearchResults(searchBar.Text).OrderBy(varName => varName.Nombre).ToList();
            }
            else
            {
                lsvActividades.ItemsSource = Actividades;
            }
        }

        void OrdenAlfabeticoDescendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                if (searchBar.Text == null)
                    searchBar.Text = "";
                lsvActividades.ItemsSource = GetSearchResults(searchBar.Text).OrderByDescending(varName => varName.Nombre).ToList();
            }
            else
            {
                lsvActividades.ItemsSource = Actividades;
            }
        }

        void DistanciaAscendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                var localizacionUsuario = Geolocation.GetLocationAsync();

            }
            else
            {
                lsvActividades.ItemsSource = Actividades;
            }
        }

        void DistanciaDescendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {

            }
            else
            {
                lsvActividades.ItemsSource = Actividades;
            }
        }
        
        void ValoracionAscendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {

            }
            else
            {
                lsvActividades.ItemsSource = Actividades;
            }
        }

        void ValoracionDescendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {

            }
            else
            {
                lsvActividades.ItemsSource = Actividades;
            }
        }
        
        void FechaAscendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {

            }
            else
            {
                lsvActividades.ItemsSource = Actividades;
            }
        }

        void FechaDescendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {

            }
            else
            {
                lsvActividades.ItemsSource = Actividades;
            }
        }
    }
}