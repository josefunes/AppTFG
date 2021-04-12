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
    public partial class ListaAlojamientosPueblo : ContentPage
    {
        public ListaAlojamientosPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Alojamientos de " + pueblo.Nombre;
            BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            //var bd = new ServicioBaseDatos<Ruta>();
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvAlojamientosPueblo.ItemsSource = null;
                lsvAlojamientosPueblo.ItemsSource = await FirebaseHelper.ObtenerTodosAlojamientosPueblo(pueblo.Id);
            }
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

        private async void LsvAlojamientosPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Comercio)e.SelectedItem;
                await Navigation.PushAsync(new PaginaAlojamiento(dato));
                lsvAlojamientosPueblo.SelectedItem = null;
            }
            catch (Exception)
            {
                //UserDialogs.Instance.Alert("Se ha producido un error", "", "Ok");
            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.Inicio));
            string nombre = nombreUsuario.Text;
            Usuario user = await FirebaseHelper.ObtenerUsuario(nombre);
            Pueblo puebloUser = await FirebaseHelper.ObtenerPueblo(user.UsuarioId);
            //var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new PaginaAlojamiento(new Comercio() { IdPueblo = puebloUser.Id }));
        }
    }
}