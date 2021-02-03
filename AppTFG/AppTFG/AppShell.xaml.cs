using AppTFG.Paginas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Xamarin.Forms.Shell
    {
        Dictionary<string, Type> routes = new Dictionary<string, Type>();
        public Dictionary<string, Type> Routes { get { return routes; } }

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            BindingContext = this;
        }

        void RegisterRoutes()
        {
            //routes.Add("pueblodescription", typeof(PuebloDetailsView));

            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }

        //private async void OnMenuItemClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new LoginPage());
        //}
    }
}