﻿using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaVistaListaFotos : TabbedPage
    {
        public PaginaVistaListaFotos(Pueblo pueblo)
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
                indicador.HeightRequest = 0;
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
                await Navigation.PushAsync(new PaginaVistaFoto(dato));
                lsvFotosPueblo.SelectedItem = null;
            }
            catch (Exception)
            {
                //UserDialogs.Instance.Alert("Se ha producido un error", "", "Ok");
            }
        }
    }
}