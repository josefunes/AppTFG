using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        //        UserDialogs.Instance.ShowLoading("Guardando ...");
        //    }
        //    else
        //    {
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
            //El nombre es null cuando el usuario pulsa el botón de Cancelar.
            if (nombre == null)
            { return; }
            Pin nuevoPin = new Pin();
            Position nuevaPosicion = new Position();
            if (Ruta.Camino == null)
            {
                Ruta.Camino = new List<Posicion>();
            }
            if (Ruta.Ubicaciones == null)
            {
                Ruta.Ubicaciones = new List<Ubicacion>();
            }
            if (nombre.Length == 0)
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
                double x = nuevoPin.Position.Latitude;
                double y = nuevoPin.Position.Longitude;
                Ubicacion ubicacion = new Ubicacion(Ruta.Id, nombre, x, y);
                Posicion posicion = new Posicion(Ruta.Id, x, y);
                Ruta.Camino.Add(posicion);
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

        private void BtnBorrarUltPin_Clicked(object sender, EventArgs e)
        {
            if (Map.Pins.Any())
            {
                if (Map.Pins.Last().Label.Equals(Ruta.Ubicaciones.Last().Nombre))
                {
                    Map.Pins.Remove(Map.Pins.Last());
                    Ruta.Ubicaciones.Remove(Ruta.Ubicaciones.Last());
                }
            }
        }

        private void BtnBorrarUltCamino_Clicked(object sender, EventArgs e)
        {
            if((Ruta.Camino.Last().X.Equals(Union.Geopath.Last().Latitude)) && (Ruta.Camino.Last().Y.Equals(Union.Geopath.Last().Longitude)))
            {
                //Se elimina el último camino creado y almacenado
                Ruta.Camino.Remove(Ruta.Camino.Last());
                //Al borrar hago que la posición vuelva a ser la misma que antes de hacer el camino
                actualPosition = Union.Geopath.First();
                int x = Ruta.Camino.Count - 1;
                var posicion1 = new Position(Ruta.Camino.ElementAt(x).X, Ruta.Camino.ElementAt(x).Y);
                Union = new Polyline()
                {
                    StrokeColor = Color.Black,
                    StrokeWidth = 12,
                    Geopath =
                            {
                                posicion1,
                                actualPosition
                            }
                };
                Ruta.Camino.Remove(Ruta.Camino.Last());
                Map.MapElements.Remove(Union);
            }
        }

        private void BtnBorrarPin_Clicked(object sender, EventArgs e)
        {
            if (Map.Pins.Any())
            {
                Ruta.Ubicaciones.Clear();
                Map.Pins.Clear();
            }
        }

        private void BtnBorrarCamino_Clicked(object sender, EventArgs e)
        {
            Ruta.Camino.Clear();
            Map.MapElements.Clear();
        }

        private void BtnBorrarTodo_Clicked(object sender, EventArgs e)
        {
            Ruta.Ubicaciones.Clear();
            Ruta.Camino.Clear();
            Map.MapElements.Clear();
            Map.Pins.Clear();
        }
    }
}