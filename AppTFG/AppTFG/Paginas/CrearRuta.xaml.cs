using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using Plugin.AudioRecorder;
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
        Position actualPosition;
        private Polyline Polyline;
        private Polyline Union;
        AudioPlayer player;
        Audio UltimoAudioPulsado;

        public CrearRuta(Ruta ruta)
        {
            InitializeComponent();
            Ruta = ruta;
            BindingContext = ruta;
            player = new AudioPlayer();
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

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var ruta = await FirebaseHelper.ObtenerRuta(Ruta.Id);
            if(ruta.Audios == null)
            {
                lsvAudios.IsVisible = false;
                lsvDescripciones.IsVisible = false;
            }
            else
            {
                lsvAudios.ItemsSource = ruta.Audios.OrderBy(o => o.Numero);
                lsvDescripciones.ItemsSource = ruta.Audios.OrderBy(o => o.Numero);
            }
            Loading(false);
        }

        void Loading(bool mostrar)
        {
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
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.5));
            Map = new Map(mapSpan)
            {
                WidthRequest = -1,
                HasScrollEnabled = true,
                HasZoomEnabled = true,
                IsShowingUser = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            PonerRuta();
            PonerPins();
            stackMapa.Children.Add(Map);
        }

        void PonerRuta()
        {
            if (Ruta.Camino == null)
            {
                return;
            }
            Position position1;
            Position position2;
            for (int i = 0; i < Ruta.Camino.Count; i++)
            {
                if (i + 1 < Ruta.Camino.Count)
                {
                    Posicion posicion1 = Ruta.Camino[i];
                    Posicion posicion2 = Ruta.Camino[i + 1];
                    position1 = new Position(posicion1.X, posicion1.Y);
                    position2 = new Position(posicion2.X, posicion2.Y);
                    Polyline = new Polyline
                    {
                        StrokeColor = Color.Black,
                        StrokeWidth = 15,
                        Geopath =
                        {
                            position1,
                            position2
                        }
                    };
                    Map.MapElements.Add(Polyline);
                }
            }
        }

        void PonerPins()
        {
            if (Ruta.Ubicaciones == null)
            {
                return;
            }
            Pin pin1;
            for (int i = 0; i < Ruta.Ubicaciones.Count; i++)
            {
                var ubicacion1 = Ruta.Ubicaciones[i];
                Position position = new Position(ubicacion1.Latitud, ubicacion1.Longitud);
                pin1 = new Pin()
                {
                    Position = position,
                    Label = ubicacion1.Nombre,
                    Type = PinType.Place
                };
                Map.Pins.Add(pin1);
            }
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

        async void OnTapCrearPin(object sender, MapClickedEventArgs args)
        {
            string nombre = await DisplayPromptAsync("Título", "Introduce número y título", "Añadir", "Cancelar", placeholder: "Por ejemplo: 1. Alcazaba");
            //El nombre es null cuando el usuario pulsa el botón de Cancelar.
            if (nombre == null)
            { return; }
            Pin nuevoPin = new Pin();
            if (Ruta.Ubicaciones == null)
            {
                Ruta.Ubicaciones = new List<Ubicacion>();
            }
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
            Ruta.Ubicaciones.Add(ubicacion);
            if (nuevoPin.Position != new Position())
            {
                Map.Pins.Add(nuevoPin);
            }
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var ruta = (Ruta)BindingContext;
            var rutaActualizada = await FirebaseHelper.ObtenerRuta(Ruta.Id);
            var audios = rutaActualizada.Audios;
            if (ruta.Id > 0)
            {
                //Actualiza los audios que se hayan guardado en la otra pantalla, pero no hayan tenido efecto en esta
                await FirebaseHelper.ActualizarRuta(ruta.Id, ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones, audios);
            }
            else
            {
                await FirebaseHelper.InsertarRuta(ruta.Id = Constantes.GenerarId(), ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones, audios);
            }
            Loading(false);
            UserDialogs.Instance.Alert("Registro realizado correctamente", "Correcto", "OK");
            //await Navigation.PopAsync();
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
            if(Ruta.Camino == null || Union == null)
            {
                return;
            }
            else if((Ruta.Camino.Last().X.Equals(Union.Geopath.Last().Latitude)) && (Ruta.Camino.Last().Y.Equals(Union.Geopath.Last().Longitude)))
            {
                //Se elimina el último camino creado y almacenado
                Ruta.Camino.Remove(Ruta.Camino.Last());
                Map.MapElements.Remove(Union);
                //Al borrar hago que la posición vuelva a ser la misma que antes de hacer el camino
                actualPosition = Union.Geopath.First();
            }
            else
            {
                return;
            }
        }

        private void BtnBorrarPin_Clicked(object sender, EventArgs e)
        {
            if (Map.Pins.Any())
            {
                if (Ruta.Ubicaciones == null) { }
                else
                {
                    Ruta.Ubicaciones.Clear();

                }
                Map.Pins.Clear();
            }
        }

        private void BtnBorrarCamino_Clicked(object sender, EventArgs e)
        {
            if (Ruta.Camino == null) { }
            else
            {
                Ruta.Camino.Clear();
            }
            Map.MapElements.Clear();
        }

        private void BtnBorrarTodo_Clicked(object sender, EventArgs e)
        {
            if (Ruta.Ubicaciones == null || Ruta.Camino == null) { }
            else
            {
                Ruta.Ubicaciones.Clear();
                Ruta.Camino.Clear();
            }
            actualPosition = new Position();
            Map.MapElements.Clear();
            Map.Pins.Clear();
        }

        private void BtnPoner1Pin_Clicked(object sender, EventArgs e)
        {
            Map.MapClicked += OnTapCrearPin;
            pagina.BackgroundColor = Color.White;
        }

        private void BtnPonerRuta_Clicked(object sender, EventArgs e)
        {
            Map.MapClicked += OnTapCrearRuta;
            pagina.BackgroundColor = Color.Black;
        }

        private void BtnInfo_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.Alert("• Al seleccionar el botón Poner Pin, se podrá pulsar en cualquier parte del mapa " +
                "para colocar un Pin en el lugar seleccionado, sin necesidad de que se se cree un camino (línea en el mapa). " +
                "Al pulsar sobre el mapa aparecerá una ventana emergente en la que se tendrá que introducir el nombre asociado.\n" +
                "\n• Al seleccionar el botón Poner Ruta, se podrá pulsar en cualquier parte del mapa " +
                "para colocar un Pin en el lugar seleccionado, que será unido mediante un camino (línea en el mapa)," +
                "al siguiente punto que se seleccione en el mapa. En este caso, solo se crea un Pin al introducir un nombre." +
                "Si no se introduce nombre, al pulsar sobre el botón de añadir, se creará un camino entre el punto seleccionado " +
                "anteriormente y el actual.\n" +
                "\n• Al pulsar sobre el botón Borrar último Pin, se eliminará el último Pin creado, sin importar si fue creado" +
                "introduciendo un Pin solo o creando un camino. Si se sigue pulsando irá eliminando Pins desde el último que se creó" +
                "hasta el primero.\n" +
                "\n• Al pulsar en el botón Borrar pins, se eliminarán todos los Pins creados.\n" +
                "\n• Al pulsar el botón Borrar último camino, se eliminará el último camino creado, SOLO el último creado." +
                "Si se ha eliminado el último camino creado, este botón no tendrá efecto hasta que se agregue otro camino.\n" +
                "\n• Al pulsar sobre Borrar Ruta, se eliminarán todos los caminos creados.\n" +
                "\n• Al pulsar Borrar Todo se borrará todo lo creado en el mapa, NO se eliminará la Ruta." +
                "Para eliminar la Ruta, hay que pulsar el botón Eliminar, que se encuentra en la página anterior.\n" +
                "\n• Al pulsar en Añadir Info, pasarás a la pantalla en la que se pueden introducir tanto explicaciones escritas como " +
                "mediante grabaciones de audio de los puntos de interés que aparecen en la Ruta.", "Info Botones", "OK");
        }

        private async void LsvDescripciones_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Audio)e.SelectedItem;
                await Navigation.PushAsync(new SubirAudio(dato));
                //lsvDescripciones.SelectedItem = null;
            }
            catch (Exception)
            {
                //UserDialogs.Instance.Alert("Se ha producido un error", "", "Ok");
            }
        }

        private void LsvAudios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                UltimoAudioPulsado = new Audio();
                var seleccionado = (Audio)e.SelectedItem;
                if (UltimoAudioPulsado.Id.Equals(seleccionado.Id))
                {
                    player.Pause();
                }
                if (seleccionado.Sonido == null)
                {
                    UserDialogs.Instance.Alert("", "", "OK");
                }
                var filePath = seleccionado.Sonido;
                player.Pause();
                if (filePath != null)
                {
                    player.Play(filePath);
                }
                UltimoAudioPulsado = seleccionado;
                lsvAudios.SelectedItem = null;
            }
            catch (Exception)
            {
                //UserDialogs.Instance.Alert("Se ha producido un error", "", "Ok");
            }
        }

        void BtnMapa_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Text)
            {
                case "Street":
                    Map.MapType = MapType.Street;
                    break;
                case "Satélite":
                    Map.MapType = MapType.Satellite;
                    break;
                case "Híbrido":
                    Map.MapType = MapType.Hybrid;
                    break;
            }
        }

        private async void BtnNuevaExp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SubirAudio(new Audio() { IdRuta = Ruta.Id }));
        }

        void Play_Clicked(object sender, EventArgs e)
        {
            try
            {
                Audio seleccionado = (Audio)lsvAudios.SelectedItem;
                if(seleccionado.Sonido == null)
                {
                    UserDialogs.Instance.Alert("", "", "OK");
                }
                var filePath = seleccionado.Sonido;

                if (filePath != null)
                {
                    //StopRecording();
                    player.Play(filePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}