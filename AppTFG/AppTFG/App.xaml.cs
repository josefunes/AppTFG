using AppTFG.Datos;
using AppTFG.Helpers;
using Xamarin.Forms;

namespace AppTFG
{
    public partial class App : Application
    {

        public static INavigation NavigationRef;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            NavigationRef = MainPage.Navigation;
        }

        static BaseDatos bd;

        public static BaseDatos BD
        {
            get
            {
                if (bd == null)
                {
                    bd = new BaseDatos(Constantes.NombreBD);
                }
                return bd;
            }
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
