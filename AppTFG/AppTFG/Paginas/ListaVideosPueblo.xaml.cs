using AppTFG.FormsVideoLibrary;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaVideosPueblo : ContentPage
    {
        public Video Video;
        public ListaVideosPueblo()
        {
        }
        public ListaVideosPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Videos de " + pueblo.Nombre;
            this.BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvVideosPueblo.ItemsSource = null;
                lsvVideosPueblo.ItemsSource = await FirebaseHelper.ObtenerTodosVideosPueblo(pueblo.Nombre);
            }
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        public async void LsvVideosPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Video)e.SelectedItem;
                await Navigation.PushAsync(new SubirVideo(dato));
                lsvVideosPueblo.SelectedItem = null;
            }
            catch (Exception)
            {

            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new SubirVideo(new Video() { Pueblo = pueblo }));
        }
    }
}