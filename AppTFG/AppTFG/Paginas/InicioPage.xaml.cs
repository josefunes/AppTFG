using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppTFG.Paginas
{

    public partial class InicioPage : ContentPage
    {

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

        //void Loading(bool mostrar)
        //{
        //    if (mostrar)
        //    {
        //        indicator.HeightRequest = 30;
        //    }
        //    else
        //    {
        //        indicator.HeightRequest = 0;
        //    }
        //    indicator.IsEnabled = mostrar;
        //    indicator.IsRunning = mostrar;
        //}

        async void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
            }
            else
            {
                await Task.Delay(2000);
                UserDialogs.Instance.HideLoading();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Loading(true);
            await CargarDatos();           
            Loading(false);
        }

        public async Task CargarDatos()
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.Inicio));
            string nombre = nombreUsuario.Text;
            Usuario user = await FirebaseHelper.ObtenerUsuario(nombre);
            Pueblo puebloUser = await FirebaseHelper.ObtenerPueblo(user.UsuarioId);

            Title = "Bienvenido, " + nombre;

            if (puebloUser == null)
            {
                nombrePueblo.Text = "";
                numRutas.Text = "0";
                numActividades.Text = "0";
                numFotos.Text = "0";
                numVideos.Text = "0";
                clvRutas.ItemsSource = null;
                clvActividades.ItemsSource = null;
            }
            else
            {
                nombrePueblo.Text = puebloUser.Nombre;
                var contRutas = await FirebaseHelper.ObtenerTodasRutasPueblo(puebloUser.Id);
                if (contRutas == null)
                {
                    numRutas.Text = "0";
                }
                else
                {
                    numRutas.Text = contRutas.Count + "";
                }
                var contActividades = await FirebaseHelper.ObtenerTodasActividadesPueblo(puebloUser.Id);
                if (contActividades == null)
                {
                    numActividades.Text = "0";
                }
                else
                {
                    numActividades.Text = contActividades.Count.ToString();
                }
                var contFotos = await FirebaseHelper.ObtenerTodasFotosPueblo(puebloUser.Id);
                if (contFotos == null)
                {
                    numFotos.Text = "0";
                }
                else
                {
                    numFotos.Text = contFotos.Count.ToString();
                }
                var contVideos = await FirebaseHelper.ObtenerTodosVideosPueblo(puebloUser.Id);
                if (contVideos == null)
                {
                    numVideos.Text = "0";
                }
                else
                {
                    numVideos.Text = contVideos.Count.ToString();
                }
                numVideos.Text = contVideos.Count.ToString();

                clvRutas.ItemsSource = null;
                clvRutas.ItemsSource = await FirebaseHelper.ObtenerTodasRutasPueblo(puebloUser.Id);

                clvActividades.ItemsSource = null;
                clvActividades.ItemsSource = await FirebaseHelper.ObtenerTodasActividadesPueblo(puebloUser.Id);
            }
            clvRutas.SelectedItem = null;
            clvActividades.SelectedItem = null;
        }
    }
}