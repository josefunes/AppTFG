using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.VistaModelos;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaImagenes : ContentPage
    {
        GridViewViewModel _viewModel;

        public ListaImagenes(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Fotos de " + pueblo.Nombre;
            _viewModel = new GridViewViewModel();
            this.BindingContext = pueblo;
            this.BindingContext = _viewModel;
            NavigationPage.SetHasBackButton(this, false);
            Constantes.GalleryCollection = new ObservableCollection<Imagenes>();
            SearchEntry.TextChanged += (sender, e) => SearchProjects(SearchEntry.Text);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadData();

            Loading(true);
            var pueblo = (Pueblo)this.BindingContext;
            if (pueblo != null && (Constantes.GalleryCollection != null && Constantes.GalleryCollection.Count > 0))
            {
                lsvImagenesPueblo.ItemsSource = null;
                lsvImagenesPueblo.ItemsSource = Constantes.GalleryCollection;
                lsvImagenesPueblo.ItemsSource = pueblo.Imagenes;
            }
            Loading(false);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            var pueblo = (Pueblo)this.BindingContext;
            await Navigation.PushAsync(new SubirFoto(new Imagenes() { Pueblo = pueblo }));
        }

        public void SearchProjects(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                _viewModel.ParentModels = Constantes.GalleryCollection;
            }
            else
            {
                var collection = Constantes.GalleryCollection;
                //var _collection = Constants.GalleryCollection.Where(x => (x.Title.ToLower().Contains(filter.ToLower())));
                var _collection = collection.Where(x => x.Nombre.ToLower().Contains(filter.ToLower()));
                _viewModel.ParentModels = new ObservableCollection<Imagenes>(_collection);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
    }
}
