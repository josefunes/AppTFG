using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaRuta : ContentPage
    {
        ServicioBaseDatos<Ruta> bd;
        Ruta Ruta;
        public PaginaRuta(Ruta ruta)
        {
            InitializeComponent();
            Ruta = ruta;
            this.BindingContext = ruta;
            bd = new ServicioBaseDatos<Ruta>();

            if (ruta.Id == 0)
                this.ToolbarItems.RemoveAt(1);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            var imagen = await ServicioMultimedia.SeleccionarImagen();
            Ruta.ImagenPrincipal = imagen.Path;
            imgRuta.Source = ImageSource.FromFile(imagen.Path);
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var ruta = (Ruta)this.BindingContext;

            if (ruta.Id > 0)
                await bd.Actualizar(ruta);
            else
                await bd.Agregar(ruta);

            Loading(false);
            await DisplayAlert("Correcto", "Registro realizado correctamente", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading(true);
                await bd.Eliminar(((Ruta)this.BindingContext).Id);
                Loading(false);
                await DisplayAlert("Correcto", "Registro eliminado correctamente", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}