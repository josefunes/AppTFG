using AppTFG.Modelos;
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
    public partial class PaginaVistaFoto : ContentPage
    {
        Foto Foto;
        public PaginaVistaFoto(Foto foto)
        {
            InitializeComponent();
            Foto = foto;
            BindingContext = foto;
            Title = Foto.Nombre;
        }
    }
}