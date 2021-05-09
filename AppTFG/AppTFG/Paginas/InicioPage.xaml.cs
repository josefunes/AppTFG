using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppTFG.Paginas
{

    public partial class InicioPage : ContentPage
    {
        Pueblo Pueblo;
        public static AppShell Shell { get; set; }
        public InicioPage()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                InitializeComponent();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("", "Se ha perdido la conexión a Internet. Cuando vuelva a tener conexión vuelva a intentar entrar a la app.", "OK");
                UserDialogs.Instance.Alert("Error", "Se ha perdido la conexión a Internet. Cuando vuelva a tener conexión vuelva a intentar entrar a la app.", "OK");
            }          
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Cargando usuario...");
            }
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}