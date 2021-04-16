using Acr.UserDialogs;
using AppTFG.VistaModelos;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                UserDialogs.Instance.Alert("Sin conexión a internet no es posible usar la app. Conéctate a una red y vuelve a intentarlo.", "Fallo al iniciar sesión", "OK");
            }
            InitializeComponent();
            BindingContext = new LoginView();
        }
    }
}