using AppTFG.Helpers;
using AppTFG.Paginas;
using SQLite;
using Xamarin.Forms;

namespace AppTFG
{
    public partial class App : Application
    {

        //public static INavigation NavigationRef;
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new LoginPage();
            //NavigationRef = MainPage.Navigation;
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
