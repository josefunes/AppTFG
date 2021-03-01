﻿using AppTFG.Modelos;
using AppTFG.Helpers;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppTFG.VistaModelos;
using System.Threading.Tasks;

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

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void LsvPueblos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.inicio));
            string nombre = nombreUsuario.Text;
            var user = await FirebaseHelper.ObtenerUsuario(nombre);
            try
            {
                var dato = (Pueblo)e.SelectedItem;
                if (dato.Id == user.IdPueblo)
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
            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.inicio));
            string nombre = nombreUsuario.Text;
            var user = await FirebaseHelper.ObtenerUsuario(nombre);
            if (user.UsuarioId == 0)
            {
                await Navigation.PushAsync(new PaginaPueblo(new Pueblo()));
            }
            else
            {
                Pueblo puebloUser = await FirebaseHelper.ObtenerPueblo(user.UsuarioId);
                await Navigation.PushAsync(new PaginaPueblo(puebloUser));
            }
        }
    }
}