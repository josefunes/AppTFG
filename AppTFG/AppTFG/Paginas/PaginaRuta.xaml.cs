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
    public partial class PaginaRuta : ContentPage
    {
        private Ruta Ruta;
        private Map Map;
        private Polyline Polyline;
        List<Audio> Audios = new List<Audio>();

        public PaginaRuta(Ruta ruta)
        {
            InitializeComponent();
            Ruta = ruta;
            BindingContext = ruta;
            CrearMapa();
            if (ruta.Id == 0) 
            { 
                ToolbarItems.RemoveAt(1);
                Title = "Nueva ruta";
            }
            else if (ruta.Id != 0)
            {
                Title = Ruta.Nombre;
            }
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Guardando ruta...");
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
                UserDialogs.Instance.ShowLoading("Eliminando ruta...");
            }
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void Loading2(bool mostrar)
        {
            if (mostrar)
            {
                indicator.HeightRequest = 30;
            }
            else
            {
                indicator.HeightRequest = -10;
            }
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            var imagen = await ServicioMultimedia.SeleccionarImagen();
            if (imagen != null)
            {
                Ruta.ImagenPrincipal = imagen.Path;
                Ruta.Stream = imagen.GetStream();
                imgRuta.Source = ImageSource.FromFile(imagen.Path);
            }
            else { }
        }

        async void CrearMapa()
        {
            Loading2(true);
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
                HeightRequest = 300,
                HasScrollEnabled = true,
                HasZoomEnabled = true,
                IsShowingUser = true
            };
            PonerRuta();
            PonerPins();
            stackMapa.Children.Add(Map);
            Loading2(false);
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

        async void BtnEditar_Clicked(object sender, EventArgs e)
        {
            var ruta = (Ruta)BindingContext;
            if (ruta.Id > 0)
            {
                await Navigation.PushAsync(new CrearRuta(ruta));
            }
            else
            {
                UserDialogs.Instance.Alert(Constantes.TitleRutaRequired + " Guarda la ruta con el nombre y vuelve a intentarlo.", "Advertencia", "OK");
                return;
            }
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var ruta = (Ruta)BindingContext;
            if (Ruta.Id > 0)
            {
                Ruta rutaActualizada = await FirebaseHelper.ObtenerRuta(Ruta.Id);
                Audios = rutaActualizada.Audios;
            }
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                Loading(false);
                UserDialogs.Instance.Alert(Constantes.TitleRutaRequired, "Advertencia", "OK");
                return;
            }
            if (ruta.Id > 0)
            { 
                if (ruta.Stream == null)
                {
                    await FirebaseHelper.ActualizarRuta(ruta.Id, ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones, Audios);
                }
                else
                {
                    //await FirebaseHelper.BorrarFoto("Imagen principal de " + ruta.Nombre);
                    await FirebaseHelper.ActualizarRuta(ruta.Id, ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal = await FirebaseHelper.SubirFoto(ruta.Stream, "Imagen principal de " + ruta.Nombre), ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones, Audios);
                }
            }
            else
            {
                if (ruta.Stream == null)
                {
                    await FirebaseHelper.InsertarRuta(ruta.Id = Constantes.GenerarId(), ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones, Audios);
                }
                else
                {
                    await FirebaseHelper.InsertarRuta(ruta.Id = Constantes.GenerarId(), ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal = await FirebaseHelper.SubirFoto(ruta.Stream, "Imagen principal de " + ruta.Nombre), ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones, Audios);
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
                await FirebaseHelper.EliminarRuta(Ruta.Id);
                Loading1(false);
                UserDialogs.Instance.Alert("Registro eliminado correctamente", "Correcto", "OK");
                await Navigation.PopAsync();
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
    }
}