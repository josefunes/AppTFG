using AppTFG.Modelos;
using AppTFG.Helpers;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using System.Threading.Tasks;

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
        //void Loading(bool mostrar)
        //{
        //    if (mostrar)
        //    {
        //        UserDialogs.Instance.ShowLoading("Cargando...");
        //    }
        //    else
        //    {
        //        UserDialogs.Instance.HideLoading();
        //    }
        //}
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

        private async void LsvActividades_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.inicio));
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
    }
}