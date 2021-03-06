using AppTFG.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppTFG.VistaModelos
{
    public class ListaPueblosFotosView : INotifyPropertyChanged
    {
        readonly IList<Foto> source;

        public ObservableCollection<Foto> Fotos { get; private set; }

        public Foto CurrentItem { get; set; }
        public int Position { get; set; }

        public ListaPueblosFotosView()
        {
            source = new List<Foto>();
            CreateFotoCollection();

            CurrentItem = Fotos.Skip(3).FirstOrDefault();
            OnPropertyChanged("CurrentItem");
            Position = 3;
            OnPropertyChanged("Position");
        }

        void CreateFotoCollection()
        {

            Fotos = new ObservableCollection<Foto>(source);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
