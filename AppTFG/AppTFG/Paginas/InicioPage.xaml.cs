using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppTFG.Paginas
{

    public partial class InicioPage : ContentPage
    {
        Pueblo Pueblo;
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

        public async Task<Pueblo> ObtenerPuebloUsuarioAsync()
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.Inicio));
            string nombre = nombreUsuario.Text;
            Usuario user = await FirebaseHelper.ObtenerUsuario(nombre);
            Pueblo = await FirebaseHelper.ObtenerPueblo(user.UsuarioId);
            return Pueblo;
        }

        async void PuebloClick(object sender, EventArgs e)
        {

            if(await ObtenerPuebloUsuarioAsync() != null)
            {
                await Navigation.PushAsync(new PaginaPueblo(Pueblo));
            }
            else
            {
                await Navigation.PushAsync(new PaginaPueblo(new Pueblo()));
            }
        }

        async void RutaClick(object sender, EventArgs e)
        {
            if (await ObtenerPuebloUsuarioAsync() != null)
            {
                await Navigation.PushAsync(new ListaRutasPueblo(Pueblo));
            }
            else
            {
                UserDialogs.Instance.Alert("Para insertar una nueva ruta hay que crear un Pueblo primero. En la pantalla 'Mi Pueblo' se pueden introducir los datos previos a la creación de Rutas.", "Atención", "Ok");
            }
        }

        async void ActividadClick(object sender, EventArgs e)
        {
            if (await ObtenerPuebloUsuarioAsync() != null)
            {
                await Navigation.PushAsync(new ListaActividadesPueblo(Pueblo));
            }
            else
            {
                UserDialogs.Instance.Alert("Para insertar una nueva actividad hay que crear un Pueblo primero. En la pantalla 'Mi Pueblo' se pueden introducir los datos previos a la creación de Actividades.", "Atención", "Ok");
            }
        }

        async void ComercioClick(object sender, EventArgs e)
        {
            if (await ObtenerPuebloUsuarioAsync() != null)
            {
                await Navigation.PushAsync(new ListaComerciosPueblo(Pueblo));
            }
            else
            {
                UserDialogs.Instance.Alert("Para insertar un nuevo comercio hay que crear un Pueblo primero. En la pantalla 'Mi Pueblo' se pueden introducir los datos previos a la creación de Comercios.", "Atención", "Ok");
            }
        }

        async void AlojamientoClick(object sender, EventArgs e)
        {
            if (await ObtenerPuebloUsuarioAsync() != null)
            {
                await Navigation.PushAsync(new ListaAlojamientosPueblo(Pueblo));
            }
            else
            {
                UserDialogs.Instance.Alert("Para insertar un nuevo alojamiento hay que crear un Pueblo primero. En la pantalla 'Mi Pueblo' se pueden introducir los datos previos a la creación de Alojamientos.", "Atención", "Ok");
            }
        }

        async void FotoClick(object sender, EventArgs e)
        {
            if (await ObtenerPuebloUsuarioAsync() != null)
            {
                await Navigation.PushAsync(new ListaFotosPueblo(Pueblo));
            }
            else
            {
                UserDialogs.Instance.Alert("Para insertar una nueva foto hay que crear un Pueblo primero. En la pantalla 'Mi Pueblo' se pueden introducir los datos previos a la subida de Fotos.", "Atención", "Ok");
            }
        }

        async void VideoClick(object sender, EventArgs e)
        {
            if (await ObtenerPuebloUsuarioAsync() != null)
            {
                await Navigation.PushAsync(new ListaVideosPueblo(Pueblo));
            }
            else
            {
                UserDialogs.Instance.Alert("Para insertar un nuevo vídeo hay que crear un Pueblo primero. En la pantalla 'Mi Pueblo' se pueden introducir los datos previos a la subida de Vídeos.", "Atención", "Ok");
            }
        }
    }
}