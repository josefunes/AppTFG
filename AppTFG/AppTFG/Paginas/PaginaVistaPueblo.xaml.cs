using AppTFG.FormsVideoLibrary;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using AppTFG.VistaModelos;
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
    public partial class PaginaVistaPueblo : ContentPage
    {
        Pueblo Pueblo;
        public static InicioView InicioView { get; set; }
        //Position position;
        //MapSpan mapSpan;
        //Map map;

        public PaginaVistaPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = pueblo.Nombre;
            Pueblo = pueblo;
            BindingContext = pueblo;
            if (pueblo.Id == 0)
            {
                Title = "Nuevo Pueblo";
                ToolbarItems.RemoveAt(1);
                //Corrdenadas de la ciudad de Almería
                //Pueblo.CoordenadaX = 36.8345130509077;
                //Pueblo.CoordenadaY = -2.463471054337188;
                //position = new Position(Pueblo.CoordenadaX, Pueblo.CoordenadaY);
                //mapSpan = new MapSpan(position, 0.01, 0.01);
                //map = new Map(mapSpan);
            }
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            var imagen = await ServicioMultimedia.SeleccionarImagen();
            Pueblo.ImagenPrincipal = imagen.Path;
            imgPueblo.Source = ImageSource.FromFile(imagen.Path);
        }

        async void BtnRutas_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new ListaRutasPueblo(pueblo));
        }

        async void BtnActividades_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new ListaActividadesPueblo(pueblo));
        }

        async void BtnFotos_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new ListaFotosPueblo(pueblo));
        }

        async void BtnVideo_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new ListaVideosPueblo(pueblo));
        }

        void BtnMapa_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Text)
            {
                case "Street":
                    map.MapType = MapType.Street;
                    break;
                case "Satélite":
                    map.MapType = MapType.Satellite;
                    break;
                case "Híbrido":
                    map.MapType = MapType.Hybrid;
                    break;
            }
        }

        //private async void BtnCrearMapa_Clicked(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(Pueblo.Nombre))
        //    {
        //        await DisplayAlert("Advertencia", Constantes.TitlePuebloRequired, "OK");
        //        return;
        //    }
        //    Pueblo.CoordenadaX = double.Parse(txtCoordenadaX.Text);
        //    Pueblo.CoordenadaY = double.Parse(txtCoordenadaY.Text);
        //    Position position = new Position(Pueblo.CoordenadaX, Pueblo.CoordenadaY);
        //    MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
        //    map = new Map(mapSpan);
        //    Pin pin = new Pin
        //    {
        //        Label = Pueblo.Nombre,
        //        Type = PinType.Place,
        //        Position = position
        //    };
        //    map.Pins.Add(pin);
        //}
    }
}