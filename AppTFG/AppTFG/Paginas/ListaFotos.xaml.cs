using AppTFG.Modelos;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaFotos : ContentPage
    {
        public ListaFotos(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Fotos de " + pueblo.Nombre;
            this.BindingContext = pueblo;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var pueblo = (Pueblo)this.BindingContext;
            if (pueblo != null)
            {
                lsvFotosPueblo.ItemsSource = null;
                lsvFotosPueblo.ItemsSource = pueblo.Fotos;
            }
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void LsvFotosPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Foto)e.SelectedItem;
                await Navigation.PushAsync(new SubirFoto(dato));
                lsvFotosPueblo.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)this.BindingContext;
            await Navigation.PushAsync(new SubirFoto(new Foto() { Pueblo = pueblo }));
        }
    }
}