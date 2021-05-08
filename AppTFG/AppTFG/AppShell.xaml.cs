using Acr.UserDialogs;
using AppTFG.Paginas;
using AppTFG.VistaModelos;
using ImageCircle.Forms.Plugin.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public static InicioView Inicio { get; set; }

        public AppShell(string nombre)
        {
            InitializeComponent();
            FlyoutHeaderTemplate = menuHeader;
            Inicio = new InicioView(nombre);
            BindingContext = Inicio;
        }

        DataTemplate menuHeader = new DataTemplate(() =>
        {
            var stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            stack.SetDynamicResource(StackLayout.BackgroundColorProperty, key: "PageBackgroundColor");
            Grid grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = 150 },
                    new ColumnDefinition { Width = 150 }
                }
            };

            Label titulo = new Label()
            {
                Text = "TRAPP",
                FontSize = 40,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BSDSemiBold",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            titulo.SetDynamicResource(Label.TextColorProperty, key: "PrimaryTextColor");

            CircleImage imagen = new CircleImage()
            {
                Source = "logo.png",
                BorderThickness = 1,
                BorderColor = Color.Black,
                WidthRequest = 70,
                HeightRequest = 70,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(-10, 10, -10, -10)
            };
            imagen.SetDynamicResource(CircleImage.BorderColorProperty, key: "PrimaryTextColor");

            Label nombreUsuario = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BSDLight",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            nombreUsuario.SetDynamicResource(Label.TextColorProperty, key: "PrimaryTextColor");
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: Inicio));

            grid.Children.Add(titulo, 0, 0);
            Grid.SetRowSpan(titulo, 2);
            grid.Children.Add(imagen, 1, 0);
            grid.Children.Add(nombreUsuario, 1, 1);

            stack.Children.Add(grid);
            return stack;
        });

        private async void MenuItem_Clicked(object sender, System.EventArgs e)
        {
            var salir = await UserDialogs.Instance.ConfirmAsync("Si pulsa sí, se cerrará su sesión actual. ¿Está seguro de que desea cerra su sesión?", "Advertencia", "Sí", "No");
            FlyoutIsPresented = false;
            if (salir == true)
            {
                await Navigation.PushAsync(new LoginPage());
            }            
        }
    }
}