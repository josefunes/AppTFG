using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using Plugin.Media;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubirFoto : ContentPage
    {
        string filePath;
        ServicioBaseDatos<Imagenes> bd;
        Imagenes Imagen;
        public SubirFoto(Imagenes imagen)
        {
            InitializeComponent();
            Imagen = imagen;
            this.BindingContext = imagen;
            bd = new ServicioBaseDatos<Imagenes>();

            if (imagen.Id == 0)
                this.ToolbarItems.RemoveAt(1);

            Title = Constantes.AppUploadTitle;
        }

        async void BtnAbrirGaleria_Clicked(object sender, EventArgs e)
        {
            PickImage();
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        async void BtnSubirFoto_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TittleEntry.Text))
            {
                await DisplayAlert(Constantes.AppTitle, Constantes.TitleRequired, Constantes.Ok);
                return;
            }
            var imagen = new Imagenes
            {
                Nombre = TittleEntry.Text,
                Path = filePath,
            };
            Constantes.GalleryCollection.Add(imagen);
            await App.NavigationRef.PopAsync();

            Loading(true);
            var image = (Imagenes)this.BindingContext;

            if (image.Id > 0)
                await bd.Actualizar(image);
            else
                await bd.Agregar(image);

            Loading(false);
            await DisplayAlert("Correcto", "Registro realizado correctamente", "OK");
            await Navigation.PopAsync();
        }

        async void PickImage()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert(Constantes.AppTitle, Constantes.PermissionDenied, Constantes.Ok);
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight
            });
            if (file == null)
                return;
            image.Source = file.Path;
            filePath = file.Path;
        }
    }
}
