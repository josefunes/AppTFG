using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaRutas : ContentPage
    {
        public static List<Ruta> Rutas { get; set; }
        public ListaRutas()
        {
            InitializeComponent();
            Title = "Lista de Rutas";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            Rutas = await FirebaseHelper.ObtenerTodasRutas();
            lsvRutas.ItemsSource = Rutas;
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void LsvRutas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Ruta)e.SelectedItem;
                await Navigation.PushAsync(new PaginaVistaRuta(dato));
                lsvRutas.SelectedItem = null;
            }
            catch (Exception)
            {
                //UserDialogs.Instance.Alert("Se ha producido un error", "", "Ok");
            }
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                lsvRutas.ItemsSource = Rutas;
            else
                lsvRutas.ItemsSource = GetSearchResults(e.NewTextValue);
        }

        public List<Ruta> GetSearchResults(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            var listaRutas = (List<Ruta>)lsvRutas.ItemsSource;
            return listaRutas.Where(f => f.Nombre.ToLowerInvariant().Contains(normalizedQuery)).ToList();
        }

        void OrdenAlfabeticoAscendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                if (searchBar.Text == null)
                    searchBar.Text = "";
                lsvRutas.ItemsSource = GetSearchResults(searchBar.Text).OrderBy(varName => varName.Nombre).ToList();
            }
            else
            {
                lsvRutas.ItemsSource = Rutas;
            }
        }

        void OrdenAlfabeticoDescendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                if (searchBar.Text == null)
                    searchBar.Text = "";
                lsvRutas.ItemsSource = GetSearchResults(searchBar.Text).OrderByDescending(varName => varName.Nombre).ToList();
            }
            else
            {
                lsvRutas.ItemsSource = Rutas;
            }
        }

        void DistanciaAscendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {

            }
            else
            {
                lsvRutas.ItemsSource = Rutas;
            }
        }

        void DistanciaDescendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {

            }
            else
            {
                lsvRutas.ItemsSource = Rutas;
            }
        }
        
        void ValoracionAscendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {

            }
            else
            {
                lsvRutas.ItemsSource = Rutas;
            }
        }

        void ValoracionDescendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {

            }
            else
            {
                lsvRutas.ItemsSource = Rutas;
            }
        }
    }
}