using AppTFG.Modelos;
using AppTFG.Helpers;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppTFG.VistaModelos;
using System.Threading.Tasks;
using System.Collections.Generic;
using Acr.UserDialogs;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaPueblos : ContentPage
    {
        public ListaPueblos()
        {
            Title = "Lista de Pueblos";
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            lsvPueblos.ItemsSource = await FirebaseHelper.ObtenerTodosPueblos();
            Loading(false);
        }

        //void Loading(bool mostrar)
        //{
        //    if (mostrar)
        //    {
        //        UserDialogs.Instance.ShowLoading("Cargando...");
        //    }
        //    else
        //    {;
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

        private async void LsvPueblos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.Inicio));
            string nombre = nombreUsuario.Text;
            var user = await FirebaseHelper.ObtenerUsuario(nombre);
            try
            {
                var dato = (Pueblo)e.SelectedItem;
                if (dato.Id == user.UsuarioId)
                {
                    await Navigation.PushAsync(new PaginaPueblo(dato));
                }
                else
                {
                    await Navigation.PushAsync(new PaginaVistaPueblo(dato));
                }
                lsvPueblos.SelectedItem = null;

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
            if (user.UsuarioId == 0 || puebloUser == null)
            {
                await Navigation.PushAsync(new PaginaPueblo(new Pueblo()));
            }
            else
            {
                await Navigation.PushAsync(new PaginaPueblo(puebloUser));
            }
        }
    }
}