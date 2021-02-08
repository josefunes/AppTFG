using AppTFG.Datos;
using AppTFG.Helpers;
using AppTFG.Paginas;
using SQLite;
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
        //public App()
        //{
        //    InitializeComponent();

        //    MainPage = new AppShell();
        //    NavigationRef = MainPage.Navigation;
        //}

        //private static SQLiteAsyncConnection conn;

        //static BaseDatos bd;
        //public static void SetDatabaseConnection(SQLiteAsyncConnection connection)
        //{
        //    conn = connection;
        //    bd = new BaseDatos(conn);
        //}
        //public static BaseDatos BD
        //{
        //    get { return bd; }
    //}

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
