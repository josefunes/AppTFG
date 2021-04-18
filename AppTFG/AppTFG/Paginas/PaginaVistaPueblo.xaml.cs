using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.VistaModelos;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaVistaPueblo : ContentPage
    {
        Pueblo Pueblo;
        Map map;
        private double lastScroll;

        public PaginaVistaPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = pueblo.Nombre;
            Pueblo = pueblo;
            BindingContext = pueblo;
            CrearMapa();
            stackMapa.IsVisible = true;
            stackMapa.IsEnabled = true;
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

            imgPueblo.TranslateTo(imgPueblo.TranslationX, translation / 3);
            lastScroll = e.ScrollY;
        }

        async void BtnRutas_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new PaginaVistaListaRutas(pueblo));
        }

        async void BtnActividades_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new PaginaVistaListaActividades(pueblo));
        }

        async void BtnComercios_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new PaginaVistaListaComercios(pueblo));
        }

        async void BtnAlojamientos_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new PaginaVistaListaAlojamientos(pueblo));
        }

        async void BtnFotos_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new PaginaVistaListaFotos(pueblo));
        }

        async void BtnVideo_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new PaginaVistaListaVideos(pueblo));
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

        public async void CrearMapa()
        {
            var nombrePueblo = Pueblo.Nombre;
            Geocoder geoCoder = new Geocoder();
            string address = nombrePueblo + ", Andalucía, Spain";
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(address);
            Position position = approximateLocations.FirstOrDefault();
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            map = new Map(mapSpan)
            {
                WidthRequest = -1,
                HeightRequest = 300,
                HasScrollEnabled = true,
                HasZoomEnabled = true
            };
            Pin pin = new Pin
            {
                Label = nombrePueblo,
                Type = PinType.Place,
                Position = position
            };
            map.Pins.Add(pin);
            stackMapa.Children.Add(map);
        }
    }
}