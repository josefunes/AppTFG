﻿using AppTFG.Modelos;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaRutasPueblo : ContentPage
    {
        public ListaRutasPueblo(Pueblo pueblo)
        {
            InitializeComponent();

            this.BindingContext = pueblo;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var pueblo = (Pueblo)this.BindingContext;
            if (pueblo != null)
            {
                lsvRutasPueblo.ItemsSource = null;
                lsvRutasPueblo.ItemsSource = pueblo.Rutas;
            }
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void LsvRutasPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Ruta)e.SelectedItem;
                await Navigation.PushAsync(new PaginaRuta(dato));
                lsvRutasPueblo.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)this.BindingContext;
            await Navigation.PushAsync(new PaginaRuta(new Ruta() { Pueblo = pueblo }));
        }
    }
}