using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppTFG.Paginas
{

    public partial class InicioPage : ContentPage
    {
        public static AppShell Shell { get; set; }
        public InicioPage()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                InitializeComponent();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("", "Se ha perdido la conexión a Internet. Cuando vuelva a tener conexión vuelva a intentar entrar a la app.", "OK");
                UserDialogs.Instance.Alert("Error", "Se ha perdido la conexión a Internet. Cuando vuelva a tener conexión vuelva a intentar entrar a la app.", "OK");
            }          
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
            }
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        protected async override void OnAppearing()
        {
            Loading(true);
            List<Pueblo> listaPueblos = await FirebaseHelper.ObtenerTodosPueblos();
            clvPueblos.ItemsSource = null;
            if (listaPueblos.Count >= 3)
                clvPueblos.ItemsSource = listaPueblos.GetRange(0, 3);
            else if (listaPueblos.Count == 0)
            {
                listaPueblos = new List<Pueblo>();
                clvPueblos.ItemsSource = listaPueblos;
                UserDialogs.Instance.Alert("Se ha producido un error al cargar la lista de Pueblos.", "Atención", "Vale");
            }
            else
                clvPueblos.ItemsSource = listaPueblos.GetRange(0, listaPueblos.Count);

            List<Ruta> listaRutas = await FirebaseHelper.ObtenerTodasRutas();
            clvRutas.ItemsSource = null;
            if (listaRutas.Count >= 3)
                clvRutas.ItemsSource = listaRutas.GetRange(0, 3);
            else if (listaRutas.Count == 0)
            {
                listaRutas = new List<Ruta>();
                clvRutas.ItemsSource = listaRutas;
                UserDialogs.Instance.Alert("Se ha producido un error al cargar la lista de Rutas.", "Atención", "Vale");
            }
            else
                clvRutas.ItemsSource = listaRutas.GetRange(0, listaRutas.Count);

            List<Actividad> listaActividades = await FirebaseHelper.ObtenerTodasActividades();
            clvActividades.ItemsSource = null;
            if (listaActividades.Count >= 3)
                clvActividades.ItemsSource = listaActividades.GetRange(0, 3);
            else if (listaActividades.Count == 0)
            {
                listaActividades = new List<Actividad>();
                clvActividades.ItemsSource = listaActividades;
                UserDialogs.Instance.Alert("Se ha producido un error al cargar la lista de Actividades.", "Atención", "Vale");
            }
            else
                clvActividades.ItemsSource = listaActividades.GetRange(0, listaActividades.Count);

            clvPueblos.SelectedItem = null;
            clvRutas.SelectedItem = null;
            clvActividades.SelectedItem = null;
            Loading(false);
        }

    }
}