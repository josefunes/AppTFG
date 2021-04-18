using AppTFG.Modelos;
using AppTFG.Helpers;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaFotosPueblo : TabbedPage
    {
        public ListaFotosPueblo(Pueblo pueblo)
        {
            InitializeComponent();

            Title = "Fotos de " + pueblo.Nombre;
            this.BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                clvFotosPueblo.ItemsSource = null;
                clvFotosPueblo.ItemsSource = await FirebaseHelper.ObtenerTodasFotosPueblo(pueblo.Id);
            }
            if (pueblo != null)
            {
                lsvFotosPueblo.ItemsSource = null;
                lsvFotosPueblo.ItemsSource = await FirebaseHelper.ObtenerTodasFotosPueblo(pueblo.Id);
            }
        }

        private async void LsvFotosPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Foto)e.SelectedItem;
                await Navigation.PushAsync(new SubirFoto(dato));
                lsvFotosPueblo.SelectedItem = null;
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
            if(puebloUser == null)
            {
                UserDialogs.Instance.Alert("Es necesario registrar, al menos, el nombre del pueblo para comenzar a introducir fotos u otros datos.","Advertencia","Ok");
                return;
            }
            await Navigation.PushAsync(new SubirFoto(new Foto() { IdPueblo = puebloUser.Id }));
        }

    }
}