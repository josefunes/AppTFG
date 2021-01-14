using AppTFG.FormsVideoLibrary;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPueblo : ContentPage
    {
        ServicioBaseDatos<Pueblo> bd;
        Pueblo Pueblo;
        public PaginaPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Pueblo = pueblo;
            this.BindingContext = pueblo;
            bd = new ServicioBaseDatos<Pueblo>();

            if (pueblo.Id == 0)
            {
                this.ToolbarItems.RemoveAt(2);
                this.ToolbarItems.RemoveAt(1);
            }
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            var imagen = await ServicioMultimedia.SeleccionarImagen();
            Pueblo.ImagenPrincipal = imagen.Path;
            imgPueblo.Source = ImageSource.FromFile(imagen.Path);
        }

        private async void BtnVideo_Clicked(object sender, EventArgs e)
        {
            var video = await ServicioMultimedia.SeleccionarVideo();
            Pueblo.VideoUrl = video.Path;
            videoPlayer.Source = VideoSource.FromFile(video.Path);
        }

        private async void BtnFotos_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)this.BindingContext;
            await Navigation.PushAsync(new ListaImagenes(pueblo));
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var pueblo = (Pueblo)this.BindingContext;

            if (pueblo.Id > 0)
                await bd.Actualizar(pueblo);
            else
                await bd.Agregar(pueblo);

            Loading(false);
            await DisplayAlert("Correcto", "Registro realizado correctamente", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading(true);
                await bd.Eliminar(((Pueblo)this.BindingContext).Id);
                Loading(false);
                await DisplayAlert("Correcto", "Registro eliminado correctamente", "OK");
                await Navigation.PopAsync();
            }
        }

        async void BtnRutas_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)this.BindingContext;
            await Navigation.PushAsync(new ListaRutasPueblo(pueblo));
        }

        async void BtnActividades_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)this.BindingContext;
            await Navigation.PushAsync(new ListaActividadesPueblo(pueblo));
        }

    }
}