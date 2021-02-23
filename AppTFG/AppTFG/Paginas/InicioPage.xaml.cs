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
        InicioView inicioView;
        Usuario usuario;
        public InicioPage(string nombre)
        {
            InitializeComponent();
            nombre = usuario.Nombre;
            inicioView = new InicioView(nombre);
            BindingContext = inicioView;
            Title = "Inicio";
        }
    }
}