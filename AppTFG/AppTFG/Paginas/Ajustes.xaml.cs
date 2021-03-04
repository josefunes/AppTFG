using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.VistaModelos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ajustes : ContentPage
    {
        AjustesView ajustesView;
        public Ajustes()
        {
            InitializeComponent();
            ajustesView = new AjustesView();
            BindingContext = ajustesView;
        }
    }
}