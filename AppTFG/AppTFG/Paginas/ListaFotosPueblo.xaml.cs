using AppTFG.Modelos;
using AppTFG.Helpers;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            Loading(true);
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
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                indicator.HeightRequest = 30;
                indicador.HeightRequest = 30;
            }
            else
            {
                indicator.HeightRequest = 0;
                indicator.HeightRequest = 0;
            }
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
            indicador.IsEnabled = mostrar;
            indicador.IsRunning = mostrar;
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
            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.inicio));
            string nombre = nombreUsuario.Text;
            Usuario user = await FirebaseHelper.ObtenerUsuario(nombre);
            Pueblo puebloUser = await FirebaseHelper.ObtenerPueblo(user.UsuarioId);
            //var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new SubirFoto(new Foto() { IdPueblo = puebloUser.Id }));
        }

    }
}