using AppTFG.Helpers;
using AppTFG.Paginas;
using System.ComponentModel;
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
                return new Command(() => { App.Current.MainPage.Navigation.PushAsync(new Registrarse()); });
            }
        }

        private async void Login()
        {
            //null or empty field validation, check weather email and password is null or empty

            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Password))
                await Application.Current.MainPage.DisplayAlert("Campos vacíos", "Por favor introduce un Usuario y una Contraseña", "OK");
            else
            {
                //call GetUser function which we define in Firebase helper class
                var user = await FirebaseHelper.ObtenerUsuario(Nombre);
                //firebase return null valuse if user data not found in database
                if (user != null)
                    if (Nombre == user.Nombre && Password == user.Password)
                    {
                        //await Application.Current.MainPage.DisplayAlert("Login Success", "", "Ok");
                        //Navigate to Wellcom page after successfuly login
                        //pass user email to welcom page
                        await Application.Current.MainPage.Navigation.PushAsync(new InicioPage(Nombre));
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Fallo al iniciar sesión", "Por favor, iontroduzca un nombre de usuario y una contraseña correctos", "OK");
                else
                    await Application.Current.MainPage.DisplayAlert("Fallo al iniciar sesión", "El Usuario introducido no existe", "OK");
            }
        }
    }
}
