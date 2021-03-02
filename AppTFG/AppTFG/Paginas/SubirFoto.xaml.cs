using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubirFoto : ContentPage
    {
        //ServicioBaseDatos<Foto> bd;
        Stream Stream;
        Foto Foto;
        public SubirFoto(Foto foto)
        {
            InitializeComponent();
            Foto = foto;
            BindingContext = foto;
            //bd = new ServicioBaseDatos<Foto>();

            if (foto.Id == 0)
            {
                this.ToolbarItems.RemoveAt(1);
                Title = "Nueva foto";
            }
            else
            {
                Title = Foto.Nombre;
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
            Foto.Imagen = imagen.Path;
            Stream = imagen.GetStream();
            imgFoto.Source = ImageSource.FromFile(imagen.Path);
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var foto = (Foto)BindingContext;
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                await DisplayAlert("Advertencia", Constantes.TitleImagenRequired, "OK");
                return;
            }
            if (foto.Id > 0)
            {
                //await bd.Actualizar(foto);
                await FirebaseHelper.ActualizarFoto(foto.Id, foto.Nombre, foto.Imagen);
            } 
            else
            {
                //await bd.Agregar(foto);
                await FirebaseHelper.SubirFoto(Stream, foto.Nombre);
                await FirebaseHelper.InsertarFoto(foto.Id = Constantes.GenerarId(), foto.Nombre, foto.Imagen = await FirebaseHelper.CargarFoto(foto.Nombre).ConfigureAwait(false), foto.Pueblo);
            }
            Loading(false);
            await DisplayAlert("Correcto", "Registro realizado correctamente", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading(true);
                //await bd.Eliminar(((Foto)BindingContext).Id);
                await FirebaseHelper.EliminarFoto(Foto.Id);
                Loading(false);
                await DisplayAlert("Correcto", "Registro eliminado correctamente", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}