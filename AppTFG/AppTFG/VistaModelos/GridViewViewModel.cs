using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppTFG.Helpers;
using AppTFG.Modelos;

namespace AppTFG.VistaModelos
{
    /// <summary>
    /// Grid view view model.
    /// </summary>
    public class GridViewViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Imagenes> GalleryCollection;
        private int _maxColumns;
        private ObservableCollection<Imagenes> _parentModels;
        private float _tileHeight;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyname">Propertyname.</param>
        private void RaisePropertyChanged([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                if (!string.IsNullOrEmpty(propertyname))
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
                }
            }
        }

        public GridViewViewModel()
        {
            _parentModels = new ObservableCollection<Imagenes>();
            ParentModels = new ObservableCollection<Imagenes>();
            ItemTapCommand = new Command<Imagenes>(OnParentTapped);
            MaxColumns = 2;
            TileHeight = 100;
        }

        public ICommand ItemTapCommand { get; private set; }

        public int MaxColumns
        {
            get { return _maxColumns; }
            set
            {
                _maxColumns = value; RaisePropertyChanged();
            }
        }

        public ObservableCollection<Imagenes> ParentModels
        {
            get { return _parentModels; }
            set
            {
                _parentModels = value;
                RaisePropertyChanged();
            }
        }

        public float TileHeight
        {
            get { return _tileHeight; }
            set { _tileHeight = value; RaisePropertyChanged(); }
        }

        internal void LoadData()
        {
            // var galleryClass = _dbhelper.GetAllObjects<GalleryClass>();
            if (Constantes.GalleryCollection != null)
            {
                ParentModels = Constantes.GalleryCollection;
            }
        }

        private void OnParentTapped(Imagenes image)
        {
            Application.Current.MainPage.DisplayAlert(Constantes.AppTitle, "Seleccionada " + image.Nombre, "Ok");
        }
    }
}