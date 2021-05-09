using AppTFG.Paginas;
using AppTFG.Walkthrough;
using Xamarin.Forms;

namespace AppTFG
{
    public partial class App : Application
    {
        Label nombreUsuario = new Label();

        public App()
        {
            InitializeComponent();
            MainPage = new WalkthroughView();
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
