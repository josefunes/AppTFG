using AppTFG.Modelos;
using AppTFG.Helpers;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaActividadesPueblo : ContentPage
    {
        public ListaActividadesPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Actividades de " + pueblo.Nombre;
            BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvActividadesPueblo.ItemsSource = null;
                lsvActividadesPueblo.ItemsSource = await FirebaseHelper.ObtenerTodasActividadesPueblo(pueblo.Id);
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

        private async void LsvActividadesPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Actividad)e.SelectedItem;
                await Navigation.PushAsync(new PaginaActividad(dato));
                lsvActividadesPueblo.SelectedItem = null;
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
            await Navigation.PushAsync(new PaginaActividad(new Actividad() { IdPueblo = puebloUser.Id }));
        }
    }
}