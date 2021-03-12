using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using AppTFG.VistaModelos;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPueblo : ContentPage
    {
        Pueblo Pueblo;
        Map map;
        public static InicioView InicioView { get; set; }
        //Position position;
        //MapSpan mapSpan;
        //Map map;

        public PaginaPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = Pueblo.Nombre;
            Pueblo = pueblo;
            BindingContext = pueblo;
            if (pueblo.Id == 0)
            {
                Title = "Nuevo Pueblo";
                ToolbarItems.RemoveAt(1);
                stackMapa.IsVisible = false;
                stackMapa.IsEnabled = false;
            }
            else
            {
                CrearMapa();
                stackMapa.IsVisible = true;
                stackMapa.IsEnabled = true;
            }
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
            }
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            var imagen = await ServicioMultimedia.SeleccionarImagen();
            Pueblo.ImagenPrincipal = imagen.Path;
            imgPueblo.Source = ImageSource.FromFile(imagen.Path);
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var pueblo = (Pueblo)BindingContext;
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.inicio));
            string nombre = nombreUsuario.Text;
            Usuario user = await FirebaseHelper.ObtenerUsuario(nombre);
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                UserDialogs.Instance.Alert("Advertencia", Constantes.TitlePuebloRequired, "OK");
                return;
            }
            if (pueblo.Id > 0)
                await FirebaseHelper.ActualizarPueblo(pueblo.Id, pueblo.Nombre, pueblo.Descripcion, pueblo.ImagenPrincipal);
            else
            {
                await FirebaseHelper.InsertarPueblo(pueblo.Id = Constantes.GenerarId(), pueblo.Nombre, pueblo.Descripcion, pueblo.ImagenPrincipal); /*, Pueblo.IdUsuario = usuario.Result.UsuarioId*/
                await FirebaseHelper.ActualizarUsuario(nombre, user.Password, pueblo.Id);
            }
            Loading(false);
            UserDialogs.Instance.Alert("Correcto", "Registro realizado correctamente", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading(true);
                await FirebaseHelper.EliminarPueblo(Pueblo.Id);
                Loading(false);
                UserDialogs.Instance.Alert("Correcto", "Registro eliminado correctamente", "OK");
                await Navigation.PopAsync();
            }
        }

        async void BtnRutas_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            if (pueblo == null)
            {
                UserDialogs.Instance.Alert("Advertencia", Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaRutasPueblo(pueblo));
            }
        }

        async void BtnActividades_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            if (pueblo == null)
            {
                UserDialogs.Instance.Alert("Advertencia", Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaActividadesPueblo(pueblo));
            }
        }

        async void BtnFotos_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            if (pueblo == null)
            {
                UserDialogs.Instance.Alert("Advertencia", Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaFotosPueblo(pueblo));
            }
        }

        async void BtnVideo_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            if (pueblo == null)
            {
                UserDialogs.Instance.Alert("Advertencia", Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaVideosPueblo(pueblo));
            }
        }

        void BtnMapa_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Text)
            {
                case "Street":
                    map.MapType = MapType.Street;
                    break;
                case "Satélite":
                    map.MapType = MapType.Satellite;
                    break;
                case "Híbrido":
                    map.MapType = MapType.Hybrid;
                    break;
            }
        }

        async void CrearMapa()
        {
            var nombrePueblo = txtNombre.Text;
            if (string.IsNullOrEmpty(nombrePueblo))
            {
                UserDialogs.Instance.Alert("Advertencia", Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "OK");
                return;
            }
            Geocoder geoCoder = new Geocoder();
            string address = nombrePueblo + ", Andalucía, Spain";
            IEnumerable <Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(address);
            Position position = approximateLocations.FirstOrDefault();
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            map = new Map(mapSpan) 
            {
                WidthRequest = -1,
                HeightRequest = 300,
                HasScrollEnabled = true
            };
            Pin pin = new Pin
            {
                Label = nombrePueblo,
                Type = PinType.Place,
                Position = position
            };
            map.Pins.Add(pin);
            stackMapa.Children.Add(map);
        }
    }
}