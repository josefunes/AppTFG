using AppTFG.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppTFG.VistaModelos
{
    class ReproductorViewModel : BaseViewModel
    {
        private Video selectedMovie;
        public Video SelectedMovie
        {
            get { return selectedMovie; }
            set
            {
                selectedMovie = value;
                OnPropertyChanged();
            }
        }
    }
}
