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
            var bd = new ServicioBaseDatos<Foto>();
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                clvFotosPueblo.ItemsSource = null;
                clvFotosPueblo.ItemsSource = await FirebaseHelper.ObtenerTodasFotosPueblo(pueblo.Nombre);
            }
            if (pueblo != null)
            {
                lsvFotosPueblo.ItemsSource = null;
                lsvFotosPueblo.ItemsSource = await FirebaseHelper.ObtenerTodasFotosPueblo(pueblo.Nombre);
            }
            Loading(false);
        }

        void Loading(bool mostrar)
        {
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
            catch (Exception ex)
            {
            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new SubirFoto(new Foto() { Pueblo = pueblo }));
        }

    }
}