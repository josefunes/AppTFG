using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubirFoto : ContentPage
    {
        Foto Foto;
        public SubirFoto(Foto foto)
        {
            InitializeComponent();
            Foto = foto;
            BindingContext = foto;

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
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Guardando foto...");
            }
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void Loading1(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Eliminando foto...");
            }
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            var imagen = await ServicioMultimedia.SeleccionarImagen();
            if (imagen != null)
            {
                Foto.Imagen = imagen.Path;
                Foto.Stream = imagen.GetStream();
                imgFoto.Source = ImageSource.FromFile(imagen.Path);
            }
            else { }
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var foto = (Foto)BindingContext;
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                UserDialogs.Instance.Alert(Constantes.TitleImagenRequired, "Advertencia", "OK");
                Loading(false);
                return;
            }
            if (foto.Id > 0)
            {
                if (foto.Stream == null)
                {
                    await FirebaseHelper.ActualizarFoto(foto.Id, foto.Nombre, foto.Imagen, foto.IdPueblo);

                }
                else
                {
                    await FirebaseHelper.ActualizarFoto(foto.Id, foto.Nombre, foto.Imagen = await FirebaseHelper.SubirFoto(foto.Stream, foto.Nombre), foto.IdPueblo);
                }
            } 
            else
            {
                await FirebaseHelper.InsertarFoto(foto.Id = Constantes.GenerarId(), 
                    foto.Nombre, 
                    foto.Imagen = await FirebaseHelper.SubirFoto(foto.Stream, foto.Nombre), 
                    foto.IdPueblo);
            }
            Loading(false);
            UserDialogs.Instance.Alert("Registro realizado correctamente", "Correcto", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading1(true);
                await FirebaseHelper.EliminarFoto(Foto.Id);
                await FirebaseHelper.BorrarFoto(Foto.Nombre);
                Loading1(false);
                UserDialogs.Instance.Alert("Registro eliminado correctamente", "Correcto", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}