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
    public partial class PaginaVistaActividad : ContentPage
    {
        readonly Actividad Actividad;
        public PaginaVistaActividad(Actividad actividad)
        {
            InitializeComponent();
            Actividad = actividad;
            BindingContext = actividad;
            Title = Actividad.Nombre;
        }
    }
}