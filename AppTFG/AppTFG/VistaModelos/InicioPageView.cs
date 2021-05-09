using AppTFG.Modelos;
using AppTFG.Paginas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppTFG.VistaModelos
{
    public class InicioPageView : BaseViewModel
    {
        private Ruta rutaSeleccionada;
        public Ruta RutaSeleccionada
        {
            get { return rutaSeleccionada; }
            set
            {
                rutaSeleccionada = value;
                OnPropertyChanged();
            }
        }

        private Actividad actividadSeleccionada;
        public Actividad ActividadSeleccionada
        {
            get { return actividadSeleccionada; }
            set
            {
                actividadSeleccionada = value;
                OnPropertyChanged();
            }
        }

        private Foto fotoSeleccionada;
        public Foto FotoSeleccionada
        {
            get { return fotoSeleccionada; }
            set
            {
                fotoSeleccionada = value;
                OnPropertyChanged();
            }
        }
        private Foto fotoOtraSeleccionada;
        public Foto FotoOtraSeleccionada
        {
            get { return fotoOtraSeleccionada; }
            set
            {
                fotoOtraSeleccionada = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectionCommand => new Command(() =>
        {
            //if (rutaSeleccionada != null)
            //{
            //    RutaSeleccionada = rutaSeleccionada;
            //    Application.Current.MainPage.Navigation.PushAsync(new PaginaRuta(RutaSeleccionada));
            //}
            //if (actividadSeleccionada != null)
            //{
            //    ActividadSeleccionada = actividadSeleccionada;
            //    Application.Current.MainPage.Navigation.PushAsync(new PaginaActividad(ActividadSeleccionada));
            //}
            //if (fotoSeleccionada != null)
            //{
            //    FotoSeleccionada = fotoSeleccionada;
            //    Application.Current.MainPage.Navigation.PushAsync(new SubirFoto(FotoSeleccionada));
            //}
            if (fotoOtraSeleccionada != null)
            {
                FotoOtraSeleccionada = fotoOtraSeleccionada;
                Application.Current.MainPage.Navigation.PushAsync(new PaginaVistaFoto(FotoOtraSeleccionada));
            }
        });

    }
}
