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
            lsvActividades.ItemsSource = GetSearchResults(searchBar.Text);
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void LsvActividades_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.Inicio));
            string nombre = nombreUsuario.Text;
            var user = await FirebaseHelper.ObtenerUsuario(nombre);
            try
            {
                var dato = (Actividad)e.SelectedItem;
                if (dato.IdPueblo == user.UsuarioId)
                {
                    await Navigation.PushAsync(new PaginaActividad(dato));
                }
                else
                {
                    await Navigation.PushAsync(new PaginaVistaActividad(dato));
                }
                lsvActividades.SelectedItem = null;
            }
            catch (Exception)
            {
                //UserDialogs.Instance.Alert("Se ha producido un error", "", "Ok");
            }
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            lsvActividades.ItemsSource = GetSearchResults(e.NewTextValue);
        }

        public static List<Actividad> GetSearchResults(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            return Actividades.Where(f => f.Nombre.ToLowerInvariant().Contains(normalizedQuery)).ToList();
        }
    }
}