using AppTFG.VistaModelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registrarse : ContentPage
    {
        RegistroView registrarse;
        public Registrarse()
        {
            InitializeComponent();
            registrarse = new RegistroView();
            BindingContext = registrarse;
        }
    }
}