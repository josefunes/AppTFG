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
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1.5));
            Map = new Map(mapSpan)
            {
                WidthRequest = -1,
                HeightRequest = 300,
                HasScrollEnabled = true,
                IsShowingUser = true
            };
            if (PonerRuta() != null)
            {
                Map.MapElements.Add(PonerRuta());
            }
            else { }
            PonerPins();
            stackMapa.Children.Add(Map);
        }

        Polyline PonerRuta()
        {
            Position position1;
            Position position2;
            for (int i = 0; i < Ruta.Camino.Count; i++)
            {
                var posicion1 = Ruta.Camino[i];
                var posicion2 = Ruta.Camino[i + 1];
                if (posicion2 != null)
                {
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
                }
                else
                {
                    continue;
                }
            }
            return Polyline;
        }

        async void PonerPins()
        {
            Pin pin1;
            var ubicaciones = await FirebaseHelper.ObtenerTodasUbicacionesRuta(Ruta.Id);
            for (int i = 0; i < ubicaciones.Count; i++)
            {
                var ubicacion1 = ubicaciones[i];
                Position position = new Position(ubicacion1.Latitud, ubicacion1.Longitud);
                pin1 = new Pin()
                {
                    Position = position,
                    Label = ubicacion1.Nombre,
                    Type = PinType.SearchResult
                };
                Map.Pins.Add(pin1);
            }
        }
    }
}