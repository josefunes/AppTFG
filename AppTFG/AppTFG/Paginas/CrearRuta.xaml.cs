using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrearRuta : ContentPage
    {
        private Ruta Ruta;
        private Map Map;
        //private List<Posicion> Camino;
        //private List<Ubicacion> Ubicaciones;
        Position actualPosition;
        private Polyline Union;
        public CrearRuta(Ruta ruta)
        {
            InitializeComponent();
            Ruta = ruta;
            BindingContext = ruta;

            if (ruta.Id == 0)
            {
                ToolbarItems.RemoveAt(1);
                Title = "Nueva ruta";
                stackMapa.IsVisible = false;
                stackMapa.IsEnabled = false;
            }
            else
            {
                Title = Ruta.Nombre;
                CrearMapa();
                stackMapa.IsVisible = true;
                stackMapa.IsEnabled = true;
            }
        }

        //void Loading(bool mostrar)
        //{
        //    if (mostrar)
        //    {
        //        UserDialogs.Instance.ShowLoading("Cargando...");
        //    }
        //    else
        //    {
        //        await Task.Delay(2000);
        //        UserDialogs.Instance.HideLoading();
        //    }
        //}

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                indicator.HeightRequest = 30;
            }
            else
            {
                indicator.HeightRequest = 0;
            }
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        async void CrearMapa()
        {
            var pueblo = await FirebaseHelper.ObtenerPueblo(Ruta.IdPueblo);
            if (string.IsNullOrEmpty(pueblo.Nombre))
            {
                UserDialogs.Instance.Alert("Advertencia", Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "OK");
                return;
            }
            Geocoder geoCoder = new Geocoder();
            string address = pueblo.Nombre + ", Andalucía, Spain";
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(address);
            Position position = approximateLocations.FirstOrDefault();
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.75));
            Map = new Map(mapSpan)
            {
                WidthRequest = -1,
                HeightRequest = 300,
                HasScrollEnabled = true,
                HasZoomEnabled = true,
                IsShowingUser = true
            };
            Map.MapClicked += OnTapCrearRuta;
            stackMapa.Children.Add(Map);
        }

        async void OnTapCrearRuta(object sender, MapClickedEventArgs args)
        {
            string nombre = await DisplayPromptAsync("Título", "Introduce número y título", "Añadir", "Cancelar", placeholder: "Por ejemplo: 1. Alcazaba");
            Pin nuevoPin = new Pin();
            Position nuevaPosicion = new Position();
            List<Posicion> camino = new List<Posicion>();
            List<Ubicacion> ubicaciones = new List<Ubicacion>();
            if (nombre.Length == 0 || nombre == null)
            {
                nuevaPosicion = args.Position;
                var x = nuevaPosicion.Latitude;
                var y = nuevaPosicion.Longitude;
                Posicion posicion = new Posicion(Ruta.Id, x, y);
                Ruta.Camino.Add(posicion);
            }
            else
            {
                nuevoPin = new Pin
                {
                    Position = args.Position,
                    Label = nombre,
                    Type = PinType.SearchResult
                };
                
                nuevoPin.InfoWindowClicked += async (s, arg) =>
                {
                    string pinName = ((Pin)s).Label;
                    await DisplayAlert("Info Window Clicked", $"El audio que toca es {pinName}.", "Ok");
                };
                var x = nuevoPin.Position.Latitude;
                var y = nuevoPin.Position.Longitude;
                var posicion = new Posicion(Ruta.Id, x, y);
                Ruta.Camino.Add(posicion);
                var ubicacion = new Ubicacion(Ruta.Id, nombre, x, y);
                Ruta.Ubicaciones.Add(ubicacion);
            }
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if (nuevoPin.Position != new Position())
                    {
                        Map.Pins.Add(nuevoPin);
                    }
                }

                if (actualPosition != new Position())
                {
                    if (nuevaPosicion != new Position())
                    {
                        Union = new Polyline
                        {
                            StrokeColor = Color.Black,
                            StrokeWidth = 12,
                            Geopath =
                            {
                                actualPosition,
                                nuevaPosicion
                            }
                        };
                        actualPosition = nuevaPosicion;
                    }
                    else if (nuevoPin.Position != new Position())
                    {
                        Union = new Polyline
                        {
                            StrokeColor = Color.Black,
                            StrokeWidth = 15,
                            Geopath =
                            {
                                actualPosition,
                                nuevoPin.Position
                            }
                        };
                        actualPosition = nuevoPin.Position;
                    }
                    Map.MapElements.Add(Union);
                }
                else if (actualPosition == new Position())
                {
                    if (nuevaPosicion != new Position())
                    {
                        actualPosition = nuevaPosicion;
                    }
                    else if (nuevoPin.Position != new Position())
                    {
                        actualPosition = nuevoPin.Position;
                    }
                }
            }
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var ruta = (Ruta)BindingContext;
            if (ruta.Id > 0)
                await FirebaseHelper.ActualizarRuta(ruta.Id, ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones);
            else
                await FirebaseHelper.InsertarRuta(ruta.Id = Constantes.GenerarId(), ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones);
            Loading(false);
            UserDialogs.Instance.Alert("Correcto", "Registro realizado correctamente", "OK");
            await Navigation.PopAsync();
        }
    }
}