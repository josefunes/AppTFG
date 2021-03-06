using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using AppTFG.Temas;
using AppTFG.VistaModelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ajustes : ContentPage, IModalPage
    {
        AjustesView ajustesView;
        public Ajustes()
        {
            InitializeComponent();
            ajustesView = new AjustesView();
            BindingContext = ajustesView;
        }

        void OnPickerSelectionChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            Tema theme = (Tema)picker.SelectedItem;

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (theme)
                {
                    case Tema.Oscuro:
                        mergedDictionaries.Add(new TemaOscuro());
                        break;
                    case Tema.Claro:
                    default:
                        mergedDictionaries.Add(new TemaClaro());
                        break;
                }
                statusLabel.Text = $"Tema {theme} cargado.";
            }
        }

        public async Task Dismiss()
        {
            await Navigation.PopModalAsync();
        }
    }
}