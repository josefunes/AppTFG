using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using AppTFG.VistaModelos;
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
    public partial class PaginaPueblo : ContentPage
    {
        private Pueblo Pueblo;
        private Map Mapa;
        public static InicioView InicioView { get; set; }

        public PaginaPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Pueblo = pueblo;
            BindingContext = pueblo;
            if (pueblo.Id == 0)
            {
                Title = "Nuevo Pueblo";
                ToolbarItems.RemoveAt(1);
                stackMapa.IsVisible = false;
                stackMapa.IsEnabled = false;
            }
            else
            {
                Title = Pueblo.Nombre;
                CrearMapa().ConfigureAwait(true);
                stackMapa.IsVisible = true;
                stackMapa.IsEnabled = true;
            }
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Guardando pueblo...");
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
                UserDialogs.Instance.ShowLoading("Eliminando pueblo...");
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
                Pueblo.ImagenPrincipal = imagen.Path;
                Pueblo.Stream = imagen.GetStream();
                imgPueblo.Source = ImageSource.FromFile(imagen.Path);
            }
            else { }
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var pueblo = (Pueblo)BindingContext;
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.Inicio));
            string nombre = nombreUsuario.Text;
            Usuario user = await FirebaseHelper.ObtenerUsuario(nombre);
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                UserDialogs.Instance.Alert(Constantes.TitlePuebloRequired, "Advertencia", "OK");
                Loading(false);
                return;
            }
            if (pueblo.Id > 0)
            {
                if (pueblo.Stream == null)
                {
                    await FirebaseHelper.ActualizarPueblo(pueblo.Id, pueblo.Nombre, pueblo.Descripcion, pueblo.ImagenPrincipal);
                }
                else
                {
                    //await FirebaseHelper.BorrarFoto("Imagen principal de " + pueblo.Nombre);
                    await FirebaseHelper.ActualizarPueblo(pueblo.Id, pueblo.Nombre, pueblo.Descripcion, pueblo.ImagenPrincipal = await FirebaseHelper.SubirFoto(pueblo.Stream, "Imagen principal de " + pueblo.Nombre));
                }
            }    
            else
            {
                if(pueblo.Stream == null)
                {
                    await FirebaseHelper.InsertarPueblo(pueblo.Id = Constantes.GenerarId(), pueblo.Nombre, pueblo.Descripcion, pueblo.ImagenPrincipal);
                }
                else
                {
                    await FirebaseHelper.InsertarPueblo(pueblo.Id = Constantes.GenerarId(), pueblo.Nombre, pueblo.Descripcion, pueblo.ImagenPrincipal = await FirebaseHelper.SubirFoto(pueblo.Stream, "Imagen principal de " + pueblo.Nombre));
                }
                if (user.UsuarioId == 0)
                {
                    await FirebaseHelper.ActualizarUsuario(nombre, user.Password, pueblo.Id);
                }
            }
            Loading(false);
            UserDialogs.Instance.Alert("Registro realizado correctamente", "Correcto", "OK");
            //await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading1(true);
                await FirebaseHelper.EliminarPueblo(Pueblo.Id);
                FirebaseHelper.EliminarTodasActividadesPueblo(Pueblo.Id);
                FirebaseHelper.EliminarTodasFotosPueblo(Pueblo.Id);
                FirebaseHelper.EliminarTodasRutasPueblo(Pueblo.Id);
                FirebaseHelper.EliminarTodosAlojamientosPueblo(Pueblo.Id);
                FirebaseHelper.EliminarTodosComerciosPueblo(Pueblo.Id);
                FirebaseHelper.EliminarTodosVideosPueblo(Pueblo.Id);
                Loading1(false);
                UserDialogs.Instance.Alert("Registro eliminado correctamente", "Correcto", "OK");
                await Navigation.PopAsync();
            }
        }

        async void BtnRutas_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            if (pueblo == null)
            {
                UserDialogs.Instance.Alert(Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "Advertencia", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaRutasPueblo(pueblo));
            }
        }

        async void BtnActividades_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            if (pueblo == null)
            {
                UserDialogs.Instance.Alert(Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "Advertencia", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaActividadesPueblo(pueblo));
            }
        }

        async void BtnComercios_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            if (pueblo == null)
            {
                UserDialogs.Instance.Alert(Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "Advertencia", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaComerciosPueblo(pueblo));
            }
        }

        async void BtnAlojamientos_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            if (pueblo == null)
            {
                UserDialogs.Instance.Alert(Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "Advertencia", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaAlojamientosPueblo(pueblo));
            }
        }

        async void BtnFotos_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            if (pueblo == null)
            {
                UserDialogs.Instance.Alert(Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "Advertencia", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaFotosPueblo(pueblo));
            }
        }

        async void BtnVideo_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            if (pueblo == null)
            {
                UserDialogs.Instance.Alert(Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "Advertencia", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ListaVideosPueblo(pueblo));
            }
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
            var nombrePueblo = txtNombre.Text;
            if (string.IsNullOrEmpty(nombrePueblo))
            {
                UserDialogs.Instance.Alert(Constantes.TitlePuebloRequired + " A continuación guarde el nombre antes de empezar a crear contenido.", "Advertencia", "OK");
                return;
            }
            MapSpan mapSpan = await GeocoderPueblo();
            Mapa = new Map(mapSpan)
            {
                WidthRequest = -1,
                HeightRequest = 350,
                HasScrollEnabled = true,
                HasZoomEnabled = true
            };
            Pin pin = new Pin
            {
                Label = nombrePueblo,
                Type = PinType.Place,
                Position = mapSpan.Center
            };
            Mapa.Pins.Add(pin);
            stackMapa.Children.Add(Mapa);
        }

        async Task<MapSpan> GeocoderPueblo()
        {
            var nombrePueblo = txtNombre.Text;
            Geocoder geoCoder = new Geocoder();
            string address = nombrePueblo + ", Andalucía, Spain";
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(address);
            Position position = approximateLocations.FirstOrDefault();
            if (position == new Position(0, 0))
            {
                string direccion = nombrePueblo + ", Andalucía";
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