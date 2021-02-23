using AppTFG.Modelos;
using AppTFG.Helpers;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaPueblos : ContentPage
    {
        Pueblo Pueblo;
        Usuario Usuario;
        public ListaPueblos()
        {
            InitializeComponent();
            Title = "Lista de Pueblos";
            if (Pueblo.Usuario.UsuarioId == Usuario.UsuarioId)
            {
                ToolbarItems.RemoveAt(1);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            //var bd = new ServicioBaseDatos<Pueblo>();
            //lsvPueblos.ItemsSource = await bd.ObtenerTabla();
            lsvPueblos.ItemsSource = await FirebaseHelper.ObtenerTodosPueblos();
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void LsvPueblos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Pueblo)e.SelectedItem;
                await Navigation.PushAsync(new PaginaPueblo(dato));
                lsvPueblos.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaPueblo(new Pueblo()));
        }
    }
}