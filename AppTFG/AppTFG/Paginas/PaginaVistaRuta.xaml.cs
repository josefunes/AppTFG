using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaVistaRuta : ContentPage
    {
        private Ruta Ruta;
        private Map Map;
        private Polyline Polyline;
        public PaginaVistaRuta(Ruta ruta)
        {
            InitializeComponent();
            Ruta = ruta;
            BindingContext = ruta;
            Title = Ruta.Nombre;
            CrearMapa();
        }

        public async Task<string> ObtenerDireccionMapa()
        {
            var pueblo = await FirebaseHelper.ObtenerPueblo(Ruta.IdPueblo);
            if (string.IsNullOrEmpty(pueblo.Nombre))
            {
                UserDialogs.Instance.Alert("Advertencia", Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "OK");
                return null;
            }
            string address = pueblo.Nombre + ", Andalucía, Spain";
            return address;
        }

        async void CrearMapa()
        {
            string direccion = await ObtenerDireccionMapa();
            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(direccion);
            Position position = approximateLocations.FirstOrDefault();
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1.5));
            Map = new Map(mapSpan)
            {
                WidthRequest = -1,
                HeightRequest = 300,
                HasScrollEnabled = true,
                HasZoomEnabled = true,
                IsShowingUser = true
            };
            PonerRuta();
            PonerPins();
            stackMapa.Children.Add(Map);
        }

        void PonerRuta()
        {
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
                pin1.InfoWindowClicked += async (s, arg) =>
                {
                    string pinName = ((Pin)s).Label;
                    await DisplayAlert(Ruta.Nombre, $"El audio que toca es {pinName}.", "Ok");
                };
                Map.Pins.Add(pin1);
            }
        }
    }
}