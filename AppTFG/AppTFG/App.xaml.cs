using AppTFG.Helpers;
using AppTFG.Paginas;
using SQLite;
using Xamarin.Forms;

namespace AppTFG
{
    public partial class App : Application
    {
        Label nombreUsuario = new Label();

        public App()
        {
            //Comprobar();
            InitializeComponent();
            MainPage = new LoginPage();
        }

        //public void Comprobar()
        //{
        //    nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.Inicio));
        //    string nombre = nombreUsuario.Text;
        //    if (nombre != null)
        //    {
        //        MainPage = new AppShell(nombre);
        //    }
        //    else
        //    {
        //        MainPage = new LoginPage();
        //    }
        //}

        protected override void OnStart()
        {
            //Comprobar();
        }

        protected override void OnSleep()
        {
            //Comprobar();
        }

        protected override void OnResume()
        {
            //Comprobar();
        }
    }
}
