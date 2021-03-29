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
        
        public PaginaRuta(Ruta ruta)
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
            else if (ruta.Id != 0)
            {
                Title = Ruta.Nombre;
                //CrearMapa();
                //stackMapa.IsVisible = true;
                //stackMapa.IsEnabled = true;
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
                HasZoomEnabled = true,
                IsShowingUser = true
            };
            if(PonerRuta() != null)
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
            for (int i=0; i<Ruta.Camino.Count; i++)
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
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                UserDialogs.Instance.Alert(Constantes.TitleRutaRequired, "Advertencia", "OK");
                Loading(false);
                return;
            }
            if (ruta.Id > 0)
            { 
                if (ruta.Stream == null)
                {
                    await FirebaseHelper.ActualizarRuta(ruta.Id, ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones);
                }
                else
                {
                    //await FirebaseHelper.BorrarFoto("Imagen principal de " + ruta.Nombre);
                    await FirebaseHelper.ActualizarRuta(ruta.Id, ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal = await FirebaseHelper.SubirFoto(ruta.Stream, "Imagen principal de " + ruta.Nombre), ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones);
                }
            }
            else
            {
                if (ruta.Stream == null)
                {
                    await FirebaseHelper.InsertarRuta(ruta.Id = Constantes.GenerarId(), ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones);
                }
                else
                {
                    await FirebaseHelper.InsertarRuta(ruta.Id = Constantes.GenerarId(), ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal = await FirebaseHelper.SubirFoto(ruta.Stream, "Imagen principal de " + ruta.Nombre), ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones);
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
    }
}