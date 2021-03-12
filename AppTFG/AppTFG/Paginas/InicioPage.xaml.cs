using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.VistaModelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{

    public partial class InicioPage : ContentPage
    {

        public InicioPage()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                InitializeComponent();
            }
            else
            {
                //Application.Current.MainPage.DisplayAlert("", "Se ha perdido la conexión a Internet. Cuando vuelva a tener conexión vuelva a intentar entrar a la app.", "OK");
                UserDialogs.Instance.Alert("Error", "Se ha perdido la conexión a Internet. Cuando vuelva a tener conexión vuelva a intentar entrar a la app.", "OK");
            }          
        }

        async void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
            }
            else
            {
                await Task.Delay(2000);
                UserDialogs.Instance.HideLoading();
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.inicio));
            string nombre = nombreUsuario.Text;
            Usuario user = await FirebaseHelper.ObtenerUsuario(nombre);
            Pueblo puebloUser = await FirebaseHelper.ObtenerPueblo(user.UsuarioId);

            Title = "Bienvenido, " + nombre;

            if (puebloUser == null)
            {
                nombrePueblo.Text = "";
                numRutas.Text = "0";
                numActividades.Text = "0";
                numFotos.Text = "0";
                numVideos.Text = "0";
                clvRutas.ItemsSource = null;
                clvActividades.ItemsSource = null;
            }
            else
            {
                if (nombrePueblo.Text == null)
                {
                    nombrePueblo.Text = puebloUser.Nombre;
                }
                else
                {
                    nombrePueblo.Text = nombrePueblo.Text;
                }

                if (numRutas.Text == null)
                {
                    var contRutas = await FirebaseHelper.ObtenerTodasRutasPueblo(puebloUser.Id);
                    numRutas.Text = contRutas.Count + "";
                }
                else
                {
                    numRutas.Text = numRutas.Text;
                }

                if (numActividades.Text == null)
                {
                    var contActividades = await FirebaseHelper.ObtenerTodasActividadesPueblo(puebloUser.Id);
                    numActividades.Text = contActividades.Count.ToString();
                }
                else
                {
                    numActividades.Text = numActividades.Text;
                }

                if (numFotos.Text == null)
                {
                    var contFotos = await FirebaseHelper.ObtenerTodasFotosPueblo(puebloUser.Id);
                    numFotos.Text = contFotos.Count.ToString();
                }
                else
                {
                    numFotos.Text = numFotos.Text;
                }

                if (numVideos.Text == null)
                {
                    var contVideos = await FirebaseHelper.ObtenerTodosVideosPueblo(puebloUser.Id);
                    numVideos.Text = contVideos.Count.ToString();
                }
                else
                {
                    numVideos.Text = numVideos.Text;
                }

                clvRutas.ItemsSource = null;
                clvRutas.ItemsSource = await FirebaseHelper.ObtenerTodasRutasPueblo(puebloUser.Id);

                clvActividades.ItemsSource = null;
                clvActividades.ItemsSource = await FirebaseHelper.ObtenerTodasActividadesPueblo(puebloUser.Id);
            }
            clvRutas.SelectedItem = null;
            clvActividades.SelectedItem = null;
            Loading(false);
        }

        //private async Task AddComponents()
        //{
        //    Label nombreUsuario = new Label();
        //    nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.inicio));
        //    string nombre = nombreUsuario.Text;
        //    Usuario user = await FirebaseHelper.ObtenerUsuario(nombre);
        //    Pueblo puebloUser = await FirebaseHelper.ObtenerPueblo(user.UsuarioId);

        //    if (puebloUser == null)
        //    {
        //        nombrePueblo.Text = "";
        //        numRutas.Text = "0";
        //        numActividades.Text = "0";
        //        numFotos.Text = "0";
        //        numVideos.Text = "0";
        //    }

        //    nombrePueblo.Text = puebloUser.Nombre;

        //    var contRutas = await FirebaseHelper.ObtenerTodasRutasPueblo(puebloUser.Id);
        //    numRutas.Text = contRutas.Count + "";

        //    var contActividades = await FirebaseHelper.ObtenerTodasActividadesPueblo(puebloUser.Id);
        //    numActividades.Text = contActividades.Count.ToString();

        //    var contFotos = await FirebaseHelper.ObtenerTodasFotosPueblo(puebloUser.Id);
        //    numFotos.Text = contFotos.Count.ToString();

        //    var contVideos = await FirebaseHelper.ObtenerTodosVideosPueblo(puebloUser.Id);
        //    numVideos.Text = contVideos.Count.ToString();
        //}

        //private async Task AddComponents()
        //{
        //    var grid = new Grid()
        //    {
        //        RowSpacing = 0,
        //        HorizontalOptions = LayoutOptions.FillAndExpand,
        //        VerticalOptions = LayoutOptions.FillAndExpand,
        //        RowDefinitions =
        //        {
        //            new RowDefinition(),
        //            new RowDefinition { Height = GridLength.Star },
        //            new RowDefinition { Height = GridLength.Star }
        //        },
        //        ColumnDefinitions =
        //        {
        //            new ColumnDefinition(),
        //            new ColumnDefinition(),
        //            new ColumnDefinition(),
        //            new ColumnDefinition()
        //        }
        //    };

        //    Label nombreUsuario = new Label();
        //    nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.inicio));
        //    string nombre = nombreUsuario.Text;
        //    Usuario user = await FirebaseHelper.ObtenerUsuario(nombre);
        //    Pueblo puebloUser = await FirebaseHelper.ObtenerPueblo(user.UsuarioId);

        //    var nombrePueblo = new Label()
        //    {
        //        Text = puebloUser.Nombre,
        //        //BackgroundColor = Color.FromHex("#FACFCF"),
        //        BackgroundColor = Color.LightSalmon,
        //        HorizontalTextAlignment = TextAlignment.Center,
        //        VerticalTextAlignment = TextAlignment.Center,
        //        HorizontalOptions = LayoutOptions.FillAndExpand,
        //        VerticalOptions = LayoutOptions.FillAndExpand,
        //        FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label)),
        //        FontFamily = "BSDSemiBold",
        //        TextColor = Color.Black,
        //    };

        //    var rutas = new Label() 
        //    {
        //        Text = "Rutas",
        //        BackgroundColor = Color.Salmon,
        //        HorizontalOptions = LayoutOptions.Center,
        //        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
        //        FontFamily = "BSDRegular",
        //        FontAttributes = FontAttributes.Bold
        //    };
        //    rutas.SetDynamicResource(Label.TextColorProperty, key: "PrimaryTextColor");

        //    var actividades = new Label() 
        //    {
        //        Text = "Acts",
        //        BackgroundColor = Color.Salmon,
        //        HorizontalOptions = LayoutOptions.Center,
        //        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
        //        FontFamily = "BSDRegular",
        //        FontAttributes = FontAttributes.Bold
        //    };
        //    actividades.SetDynamicResource(Label.TextColorProperty, key: "PrimaryTextColor");

        //    var fotos = new Label() 
        //    {
        //        Text = "Fotos",
        //        BackgroundColor = Color.Salmon,
        //        HorizontalOptions = LayoutOptions.Center,
        //        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
        //        FontFamily = "BSDRegular",
        //        FontAttributes = FontAttributes.Bold
        //    };
        //    fotos.SetDynamicResource(Label.TextColorProperty, key: "PrimaryTextColor");

        //    var videos = new Label() 
        //    {
        //        Text = "Vídeos",
        //        BackgroundColor = Color.Salmon,
        //        HorizontalOptions = LayoutOptions.Center,
        //        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
        //        FontFamily = "BSDRegular",
        //        FontAttributes = FontAttributes.Bold
        //    };
        //    videos.SetDynamicResource(Label.TextColorProperty, key: "PrimaryTextColor");

        //    var contRutas = await FirebaseHelper.ObtenerTodasRutasPueblo(puebloUser.Id);
        //    var numRutas = new Label() 
        //    {
        //        Text = contRutas.Count.ToString()
        //    };

        //    var contActividades = await FirebaseHelper.ObtenerTodasActividadesPueblo(puebloUser.Id);
        //    var numActividades = new Label()
        //    {
        //        Text = contActividades.Count.ToString()
        //    };

        //    var contFotos = await FirebaseHelper.ObtenerTodasFotosPueblo(puebloUser.Id);
        //    var numFotos = new Label()
        //    {
        //        Text = contFotos.Count.ToString()
        //    };

        //    var contVideos = await FirebaseHelper.ObtenerTodosVideosPueblo(puebloUser.Id);
        //    var numVideos = new Label()
        //    {
        //        Text = contVideos.Count.ToString()
        //    };

        //    grid.Children.Add(nombrePueblo, 0, 0);
        //    Grid.SetColumnSpan(nombrePueblo, 4);
        //    grid.Children.Add(rutas, 0, 1);
        //    grid.Children.Add(actividades, 0, 2);
        //    grid.Children.Add(fotos, 0, 3);
        //    grid.Children.Add(videos, 0, 4);
        //    grid.Children.Add(numRutas, 1, 1);
        //    grid.Children.Add(numActividades, 1, 2);
        //    grid.Children.Add(numFotos, 1, 3);
        //    grid.Children.Add(numVideos, 1, 4);

        //    //var frame = new Frame()
        //    //{
        //    //    CornerRadius = 20,
        //    //    HorizontalOptions = LayoutOptions.StartAndExpand,
        //    //    Content = grid
        //    //};
        //    //frame.SetDynamicResource(Frame.BackgroundColorProperty, key: "PageBackgroundColor");

        //    var stackLayout = new StackLayout()
        //    {
        //        HorizontalOptions = LayoutOptions.FillAndExpand,
        //        Padding = new Thickness(5, 5, 5, 5),
        //        BackgroundColor = Color.LightGray,
        //    };
        //    stackLayout.SetDynamicResource(Label.TextColorProperty, key: "PageBackgroundColor");

        //    stackLayout.Children.Add(grid);

        //    Content = stackLayout;
        //}
    }
}