using AppTFG.Helpers;
using AppTFG.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Map = Xamarin.Forms.Maps.Map;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaVistaAlojamiento : ContentPage
    {
        private Comercio Alojamiento;
        private Map Mapa;
        private double lastScroll;
        public PaginaVistaAlojamiento(Comercio alojamiento)
        {
            InitializeComponent();
            Alojamiento = alojamiento;
            BindingContext = alojamiento;
            Title = Alojamiento.Nombre;
            CrearMapa().ConfigureAwait(true);
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

            imgAlojamiento.TranslateTo(imgAlojamiento.TranslationX, translation / 3);
            lastScroll = e.ScrollY;
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
            if (Alojamiento.Ubicacion != null)
            {
                Pin pin = new Pin
                {
                    Label = Alojamiento.Nombre,
                    Type = PinType.Place,
                    Position = new Position(Alojamiento.Ubicacion.Latitud, Alojamiento.Ubicacion.Longitud)
                };
                Mapa.Pins.Add(pin);
            }
            stackMapa.Children.Add(Mapa);
        }

        async Task<MapSpan> GeocoderPueblo()
        {
            var Pueblo = await FirebaseHelper.ObtenerPueblo(Alojamiento.IdPueblo);
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
    }
}