using AppTFG.Paginas;
using Xamarin.Forms;

namespace AppTFG
{
    public partial class App : Application
    {
        Label nombreUsuario = new Label();

        public App()
        {
            InitializeComponent();
            MainPage = new LoginPage();
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
