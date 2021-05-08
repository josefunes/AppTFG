using Acr.UserDialogs;
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
        private double lastScroll;
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
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Guardando actividad...");
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
                UserDialogs.Instance.ShowLoading("Eliminando actividad...");
            }
            else
            {

                UserDialogs.Instance.HideLoading();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            parallaxScroll.Scrolled += OnParallaxScrollScrolled;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            parallaxScroll.Scrolled -= OnParallaxScrollScrolled;
        }

        private void OnParallaxScrollScrolled(object sender, ScrolledEventArgs e)
        {
            double translation;
            if (lastScroll < e.ScrollY)
            {
                translation = 0 - ((e.ScrollY / 2));

                if (translation > 0) translation = 0;
            }
            else
            {
                translation = 0 + ((e.ScrollY / 2));

                if (translation > 0) translation = 0;
            }

            imgActividad.TranslateTo(imgActividad.TranslationX, translation / 3);
            lastScroll = e.ScrollY;
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            var imagen = await ServicioMultimedia.SeleccionarImagen();
            if (imagen != null)
            {
                Actividad.ImagenPrincipal = imagen.Path;
                Actividad.Stream = imagen.GetStream();
                imgActividad.Source = ImageSource.FromFile(imagen.Path);
            }
            else { }
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var actividad = (Actividad)BindingContext;
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                UserDialogs.Instance.Alert(Constantes.TitleActividadRequired, "Advertencia", "OK");
                Loading(false);
                return;
            }
            if (datePicker.Date.ToShortDateString() != "01/01/2021")
            {
                actividad.Fecha = datePicker.Date.ToLongDateString();
            }
            if (timePicker.Time.ToString() != "00:00:01")
            {
                actividad.Hora = timePicker.Time.ToString();
            }
            if (actividad.Id > 0)
            {
                if (actividad.Stream == null)
                {
                    await FirebaseHelper.ActualizarActividad(actividad.Id, actividad.Nombre, actividad.Descripcion, actividad.ImagenPrincipal, actividad.Fecha, actividad.Hora, actividad.VideoUrl, actividad.IdPueblo);
                }
                else
                {
                    await FirebaseHelper.ActualizarActividad(actividad.Id, actividad.Nombre, actividad.Descripcion, actividad.ImagenPrincipal = await FirebaseHelper.SubirFoto(actividad.Stream, "Imagen principal de " + actividad.Nombre), actividad.Fecha, actividad.Hora, actividad.VideoUrl, actividad.IdPueblo);
                }
            }
            else
            {
                if(actividad.Stream == null)
                {
                    await FirebaseHelper.InsertarActividad(actividad.Id = Constantes.GenerarId(), actividad.Nombre, actividad.Descripcion, actividad.ImagenPrincipal, actividad.Fecha, actividad.Hora, actividad.VideoUrl, actividad.IdPueblo);
                }
                else
                {
                    await FirebaseHelper.InsertarActividad(actividad.Id = Constantes.GenerarId(), actividad.Nombre, actividad.Descripcion, actividad.ImagenPrincipal = await FirebaseHelper.SubirFoto(actividad.Stream, "Imagen principal de " + actividad.Nombre), actividad.Fecha, actividad.Hora, actividad.VideoUrl, actividad.IdPueblo);
                }
            }
            Loading(false);
            UserDialogs.Instance.Alert("Registro realizado correctamente", "Correcto", "OK");
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading1(true);
                await FirebaseHelper.EliminarActividad(Actividad.Id);
                Loading1(false);
                UserDialogs.Instance.Alert("Registro eliminado correctamente", "Correcto", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}