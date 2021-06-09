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
    public partial class PaginaRealizacionRuta : ContentPage
    {
        private Ruta Ruta;
        private Map Map;
        private Polyline Polyline;
        AudioPlayer player;
        Audio UltimoAudioPulsado;

        public PaginaRealizacionRuta(Ruta ruta)
        {
            InitializeComponent();
            Ruta = ruta;
            BindingContext = ruta;
            Title = Ruta.Nombre;
            player = new AudioPlayer();
            CrearMapa();
            stackMapa.IsVisible = true;
            stackMapa.IsEnabled = true;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var ruta = await FirebaseHelper.ObtenerRuta(Ruta.Id);
            if (ruta.Audios == null)
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
    }
}