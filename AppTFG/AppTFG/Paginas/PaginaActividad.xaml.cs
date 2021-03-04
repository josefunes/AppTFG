using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
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
    public partial class PaginaActividad : ContentPage
    {
        readonly Actividad Actividad;
        public PaginaActividad(Actividad actividad)
        {
            InitializeComponent();
            Actividad = actividad;
            BindingContext = actividad;

            if (actividad.Id == 0)
            {
                Title = "Nueva Actividad";
                ToolbarItems.RemoveAt(1);
            }
            else
            {
                Title = Actividad.Nombre;
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
            Actividad.ImagenPrincipal = imagen.Path;
            imgActividad.Source = ImageSource.FromFile(imagen.Path);
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var actividad = (Actividad)BindingContext;
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                await DisplayAlert("Advertencia", Constantes.TitleActividadRequired, "OK");
                return;
            }
            if (actividad.Id > 0)
                await FirebaseHelper.ActualizarActividad(actividad.Id, actividad.Nombre, actividad.Descripcion, actividad.ImagenPrincipal, actividad.VideoUrl);
            else
                await FirebaseHelper.InsertarActividad(actividad.Id = Constantes.GenerarId(), actividad.Nombre, actividad.Descripcion, actividad.ImagenPrincipal, actividad.VideoUrl, actividad.IdPueblo);
            Loading(false);
            await DisplayAlert("Correcto", "Registro realizado correctamente", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading(true);
                await FirebaseHelper.EliminarActividad(Actividad.Id);
                Loading(false);
                await DisplayAlert("Correcto", "Registro eliminado correctamente", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}