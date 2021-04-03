using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Paginas;
using System.ComponentModel;
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
                await Task.Delay(2000);
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Por favor introduce un Usuario y una Contraseña", "Campos vacíos", "OK");
            }   
            else
            {
                //call GetUser function which we define in Firebase helper class
                var user = await FirebaseHelper.ObtenerUsuario(Nombre);
                //firebase return null value if user data not found in database
                if (user != null)
                {
                    if (Nombre == user.Nombre && Password == Constantes.Descifrar(user.Password))
                    {
                        await Task.Delay(2000);
                        Application.Current.MainPage = new AppShell(Nombre);
                        UserDialogs.Instance.HideLoading();
                    }
                    else if(Connectivity.NetworkAccess != NetworkAccess.Internet)
                    {
                        await Task.Delay(2000);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Sin conexión a internet no es posible usar la app. Conéctate a una red y vuelve a intentarlo.", "Fallo al iniciar sesión", "OK");
                    }
                    else
                    {
                        await Task.Delay(2000);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Por favor, introduzca un nombre de usuario y una contraseña correctos", "Fallo al iniciar sesión", "OK");
                    }
                }
                else
                {
                    await Task.Delay(2000);
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Alert("El Usuario introducido no existe", "Fallo al iniciar sesión", "OK");
                }
            }
        }
    }
}
