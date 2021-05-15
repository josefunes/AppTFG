using AppTFG.Modelos;
using AppTFG.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaPueblos : ContentPage
    {
        public static List<Pueblo> Pueblos { get; set; }
        public ListaPueblos()
        {
            Title = "Lista de Pueblos";
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            Pueblos = await FirebaseHelper.ObtenerTodosPueblos();
            lsvPueblos.ItemsSource = Pueblos;
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void LsvPueblos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Pueblo)e.SelectedItem;
                await Navigation.PushAsync(new PaginaVistaPueblo(dato));
                lsvPueblos.SelectedItem = null;
            }
            catch (Exception)
            {
                //UserDialogs.Instance.Alert("Se ha producido un error", "", "Ok");
            }
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                lsvPueblos.ItemsSource = Pueblos;
            else
                lsvPueblos.ItemsSource = GetSearchResults(e.NewTextValue);
        }

        public List<Pueblo> GetSearchResults(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            var listaPueblos = (List<Pueblo>)lsvPueblos.ItemsSource;
            return listaPueblos.Where(pueblo => pueblo.Nombre.ToLowerInvariant().Contains(normalizedQuery)).ToList();
        }

        void OrdenAlfabeticoAscendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                if (searchBar.Text == null)
                    searchBar.Text = "";
                lsvPueblos.ItemsSource = GetSearchResults(searchBar.Text).OrderBy(pueblo => pueblo.Nombre).ToList();
            }
            else
            {
                lsvPueblos.ItemsSource = Pueblos;
            }
        }

        void OrdenAlfabeticoDescendenteChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                if (searchBar.Text == null)
                    searchBar.Text = "";
                lsvPueblos.ItemsSource = GetSearchResults(searchBar.Text).OrderByDescending(pueblo => pueblo.Nombre).ToList();
            }
            else
            {
                lsvPueblos.ItemsSource = Pueblos;
            }
        }
    }
}