﻿using Acr.UserDialogs;
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
    public partial class ListaRutas : ContentPage
    {
        public ListaRutas()
        {
            InitializeComponent();
            Title = "Lista de Rutas";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            //var bd = new ServicioBaseDatos<Ruta>();
            //lsvRutas.ItemsSource = await bd.ObtenerTabla();
            lsvRutas.ItemsSource = await FirebaseHelper.ObtenerTodasRutas();
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

        private async void LsvRutas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.Inicio));
            string nombre = nombreUsuario.Text;
            var user = await FirebaseHelper.ObtenerUsuario(nombre);
            try
            {
                var dato = (Ruta)e.SelectedItem;
                if (dato.IdPueblo == user.UsuarioId)
                {
                    await Navigation.PushAsync(new PaginaRuta(dato));
                }
                else
                {
                    await Navigation.PushAsync(new PaginaVistaRuta(dato));
                }
                lsvRutas.SelectedItem = null;
            }
            catch (Exception)
            {
                //UserDialogs.Instance.Alert("Se ha producido un error", "", "Ok");
            }
        }
    }
}