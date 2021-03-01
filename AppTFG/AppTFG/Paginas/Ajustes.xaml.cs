using AppTFG.VistaModelos;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ajustes : ContentPage
    {
        public static InicioView InicioView { get; set; }
        public Ajustes() 
        {
            InitializeComponent();
            Label nombreUsuario = new Label() { };
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: InicioView));
            Title = "Ajustes";
        }
    }
}