using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Map = Xamarin.Forms.Maps.Map;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaComercio : ContentPage
    {
        private Comercio Comercio;
        private Map Mapa;
        public PaginaComercio(Comercio comercio)
        {
            InitializeComponent();
            Comercio = comercio;
            BindingContext = comercio;

            if (comercio.Id == 0)
            {
                Title = "Nuevo Comercio";
                ToolbarItems.RemoveAt(1);
            }
            else
            {
                Title = Comercio.Nombre;
            }
            CrearMapa().ConfigureAwait(true);
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Guardando comercio...");
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
                UserDialogs.Instance.ShowLoading("Eliminando comercio...");
            }
            else
            {

                UserDialogs.Instance.HideLoading();
            }
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            var imagen = await ServicioMultimedia.SeleccionarImagen();
            Comercio.ImagenPrincipal = imagen.Path;
            imgComercio.Source = ImageSource.FromFile(imagen.Path);
        }

        void BtnMapa_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Text)
            {
                case "Street":
                    Mapa.MapType = MapType.Street;
                    break;
                case "Satélite":
                    Mapa.MapType = MapType.Satellite;
                    break;
                case "Híbrido":
                    Mapa.MapType = MapType.Hybrid;
                    break;
            }
        }

        async Task CrearMapa()
        {
            MapSpan mapSpan = await GeocoderPueblo();
            Mapa = new Map(mapSpan)
            {
                WidthRequest = -1,
                HeightRequest = 350,
                HasScrollEnabled = true,
                HasZoomEnabled = true
            };
            Mapa.MapClicked += OnTapCrearPin;
            if (Comercio.Ubicacion != null)
            {
                Pin pin = new Pin
                {
                    Label = Comercio.Nombre,
                    Type = PinType.Place,
                    Position = new Position(Comercio.Ubicacion.Latitud, Comercio.Ubicacion.Longitud)
                };
                Mapa.Pins.Add(pin);
            }
            stackMapa.Children.Add(Mapa);
        }

        async Task<MapSpan> GeocoderPueblo()
        {
            var Pueblo = await FirebaseHelper.ObtenerPueblo(Comercio.IdPueblo);
            Geocoder geoCoder = new Geocoder();
            string address = Pueblo.Nombre + ", Andalucía, Spain";
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(address);
            Position position = approximateLocations.FirstOrDefault();
            if (position == new Position(0, 0))
            {
                string direccion = Pueblo + ", Andalucía";
                IEnumerable<Position> direccionesAproximadas = await geoCoder.GetPositionsForAddressAsync(direccion);
                Position posicion = direccionesAproximadas.FirstOrDefault();
                if (posicion == new Position(0, 0) || Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    stackMapa.IsVisible = false;
                    stackMapa.IsEnabled = false;
                    //Como hay que devolver un MapSpan, devuelvo la posición de Almería
                    return new MapSpan(new Position(36.834047, -2.4637136), 0.01, 0.01);
                }
                else
                {
                    return new MapSpan(posicion, 0.01, 0.01);
                }
            }
            else
            {
                return new MapSpan(position, 0.01, 0.01);
            }

        }

        async void OnTapCrearPin(object sender, MapClickedEventArgs args)
        {
            string nombre = await DisplayPromptAsync("Título", "Introduce nombre del Comercio", "Añadir", "Cancelar", placeholder: "Por ejemplo: 1. Alcazaba");
            //El nombre es null cuando el usuario pulsa el botón de Cancelar.
            if (nombre == null)
            { return; }
            Pin nuevoPin = new Pin
            {
                Position = args.Position,
                Label = nombre,
                Type = PinType.SearchResult
            };
            double x = nuevoPin.Position.Latitude;
            double y = nuevoPin.Position.Longitude;
            Ubicacion ubicacion = new Ubicacion(Comercio.Id, nombre, x, y);
            Comercio.Ubicacion = ubicacion;
            if (nuevoPin.Position != new Position())
            {
                if (Mapa.Pins.Any())
                {
                    Mapa.Pins.Clear();
                }
                Mapa.Pins.Add(nuevoPin);
            }
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var comercio = (Comercio)BindingContext;
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                UserDialogs.Instance.Alert(Constantes.TitleComercioRequired, "Advertencia", "OK");
                Loading(false);
                return;
            }
            if (comercio.Id > 0)
            {
                if (comercio.Stream == null)
                {
                    await FirebaseHelper.ActualizarComercio(comercio.Id, comercio.Nombre, comercio.Descripcion, comercio.ImagenPrincipal, comercio.Contacto, comercio.Horario, comercio.Ubicacion, comercio.VideoUrl, comercio.IdPueblo);
                }
                else
                {
                    await FirebaseHelper.ActualizarComercio(comercio.Id, comercio.Nombre, comercio.Descripcion, comercio.ImagenPrincipal = await FirebaseHelper.SubirFoto(comercio.Stream, "Imagen principal de " + comercio.Nombre), comercio.Contacto, comercio.Horario, comercio.Ubicacion, comercio.VideoUrl, comercio.IdPueblo);
                }
            }
            else
            {
                if (comercio.Stream == null)
                {
                    await FirebaseHelper.InsertarComercio(comercio.Id = Constantes.GenerarId(), comercio.Nombre, comercio.Descripcion, comercio.ImagenPrincipal, comercio.Contacto, comercio.Horario, comercio.Ubicacion, comercio.VideoUrl, comercio.IdPueblo);
                }
                else
                {
                    await FirebaseHelper.InsertarComercio(comercio.Id = Constantes.GenerarId(), comercio.Nombre, comercio.Descripcion, comercio.ImagenPrincipal = await FirebaseHelper.SubirFoto(comercio.Stream, "Imagen principal de " + comercio.Nombre), comercio.Contacto, comercio.Horario, comercio.Ubicacion, comercio.VideoUrl, comercio.IdPueblo);
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
                await FirebaseHelper.EliminarComercio(Comercio.Id);
                Loading1(false);
                UserDialogs.Instance.Alert("Registro eliminado correctamente", "Correcto", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}