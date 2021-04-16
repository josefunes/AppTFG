using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Paginas;
using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppTFG.VistaModelos
{
    public class LoginView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public LoginView()
        {

        }
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Nombre"));
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }
        public Command LoginCommand
        {
            get
            {
                return new Command(Login);
            }
        }
        public Command Registro
        {
            get
            {
                return new Command(() => { Application.Current.MainPage = new Registrarse(); });
            }
        }

        private async void Login()
        {
            UserDialogs.Instance.ShowLoading("Comprobando credenciales...");
            //null or empty field validation, check weather email and password is null or empty 
            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Password))
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Por favor introduce un Usuario y una Contraseña.", "Campos vacíos", "OK");
                return;
            }
            string nombreSinEspacios = Regex.Replace(Nombre, @"\s", "");
            string passSinEspacios = Regex.Replace(Password, @"\s", "");
            
            if (!string.IsNullOrEmpty(nombreSinEspacios))
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constantes.WebAPIkey));
                try
                {
                    var auth = await authProvider.SignInWithEmailAndPasswordAsync(nombreSinEspacios, passSinEspacios);
                    //var content = await auth.GetFreshAuthAsync();
                    //var serializedcontnet = JsonConvert.SerializeObject(content);

                    //Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);
                    Application.Current.MainPage = new AppShell(nombreSinEspacios);
                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception)
                {
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Alert("Por favor, introduzca un nombre de usuario y una contraseña correctos", "Fallo al iniciar sesión", "OK");
                }
            }
            else if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Sin conexión a internet no es posible usar la app. Conéctate a una red y vuelve a intentarlo.", "Fallo al iniciar sesión", "OK");
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("El Usuario introducido no existe.", "Fallo al iniciar sesión", "OK");
            }
        }
    }
}