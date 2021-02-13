using AppTFG.Modelos;
using AppTFG.Paginas;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppTFG.VistaModelos
{
    public class ListaVideosViewModel : BaseViewModel
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

        public ICommand PlayCommand => new Command(() =>
        {
            var vm = new ReproductorViewModel { SelectedMovie = selectedMovie };
            var page = new SubirVideo(selectedMovie) { BindingContext = vm };
            Application.Current.MainPage.Navigation.PushAsync(page);
        });
    }
}
