using AppTFG.BaseDeDatosLocal;
using AppTFG.Walkthrough;
using AppTFG.Modelos;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using AppTFG.Paginas;

namespace AppTFG
{
    public partial class App : Application
    {
        private Saltar Saltar;
        private Saltar Saltar1;
        static LocalDatabase database;

        public static LocalDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new LocalDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Trapp.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            Comprobaciones();
        }

        public async void Comprobaciones()
        {
            //var a1 = await Database.DeleteAllSaltarAsync();
            var a = await Database.GetSaltarsAsync();
            if (a.Count > 0)
                MainPage = new AppShell();
            else
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
