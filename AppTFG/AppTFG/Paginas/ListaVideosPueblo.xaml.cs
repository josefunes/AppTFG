using Acr.UserDialogs;
using AppTFG.FormsVideoLibrary;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaVideosPueblo : ContentPage
    {
        public Video Video;
        public ListaVideosPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Videos de " + pueblo.Nombre;
            BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvVideosPueblo.ItemsSource = null;
                lsvVideosPueblo.ItemsSource = await FirebaseHelper.ObtenerTodosVideosPueblo(pueblo.Id);
            }
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

        public async void LsvVideosPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Video)e.SelectedItem;
                await Navigation.PushAsync(new SubirVideo(dato));
                lsvVideosPueblo.SelectedItem = null;
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
            if (puebloUser == null)
            {
                UserDialogs.Instance.Alert("Es necesario registrar, al menos, el nombre del pueblo para comenzar a introducir vídeos u otros datos.", "Advertencia", "Ok");
                return;
            }
            await Navigation.PushAsync(new SubirVideo(new Video() { IdPueblo = puebloUser.Id }));
        }
    }
}