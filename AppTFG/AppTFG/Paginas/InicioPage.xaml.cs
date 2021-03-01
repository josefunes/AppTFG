using AppTFG.Modelos;
using AppTFG.VistaModelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{

    public partial class InicioPage : ContentPage
    {
        public static InicioView InicioView { get; set; }
        public InicioPage()
        {
            InitializeComponent();
        }
    }
}