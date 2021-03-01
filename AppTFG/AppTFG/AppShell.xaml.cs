using AppTFG.VistaModelos;
using ImageCircle.Forms.Plugin.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public static InicioView inicio { get; set; }

        public AppShell(string nombre)
        {
            InitializeComponent();
            FlyoutHeaderTemplate = menuHeader;
            inicio = new InicioView(nombre);
            BindingContext = inicio;
        }

        DataTemplate menuHeader = new DataTemplate(() =>
        {
            var stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
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
                TextColor = Color.Black,
                FontSize = 40,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BSDSemiBold",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

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

            Label nombreUsuario = new Label()
            {
                TextColor = Color.Black,
                FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BSDLight",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: inicio));

            grid.Children.Add(titulo, 0, 0);
            Grid.SetRowSpan(titulo, 2);
            grid.Children.Add(imagen, 1, 0);
            grid.Children.Add(nombreUsuario, 1, 1);

            stack.Children.Add(grid);
            return stack;
        });
    }
}