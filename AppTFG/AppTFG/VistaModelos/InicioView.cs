using AppTFG.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AppTFG.VistaModelos
{
    public class InicioView
    {
        public InicioView(string nombre2)
        {
            Nombre = nombre2;
        }
        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
    }
}
