using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Paginas;
using Firebase.Auth;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AppTFG.VistaModelos
{
    public class RegistroView : INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler PropertyChanged;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        private string confirmpassword;
        public string ConfirmPassword
        {
            get { return confirmpassword; }
            set
            {
                confirmpassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ConfirmPassword"));
            }
        }

        public Command Login
        {
            get
            {
                return new Command(() => { Application.Current.MainPage = new LoginPage(); });
            }
        }

        public Command SignUpCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Password == ConfirmPassword)
                        SignUp();
                    else
                        UserDialogs.Instance.Alert("La contraseña introducida tiene que coincidir con la anterior.", "Fallo en el registro", "OK");
                });
            }
        }

        private async void SignUp()
        {
            //null or empty field validation, check weather email and password is null or empty
            string nombreSinEspacios = Regex.Replace(Nombre, @"\s", "");
            string passSinEspacios = Regex.Replace(Password, @"\s", "");
            if (string.IsNullOrEmpty(nombreSinEspacios) || string.IsNullOrEmpty(passSinEspacios))
                UserDialogs.Instance.Alert("Por favor, introduzca un nombre de usuario y una contraseña.", "Campos vacíos", "OK");
            else if (!string.IsNullOrEmpty(nombreSinEspacios))
            {
                var user = await FirebaseHelper.ObtenerUsuario(nombreSinEspacios);
                if (user != null)
                { 
                    if (nombreSinEspacios == user.Nombre)
                    {
                        UserDialogs.Instance.Alert("Por favor, introduzca un nombre de usuario distinto.", "Usuario existente", "OK");
                    }
                }
                else if (!string.IsNullOrEmpty(Password))
                {
                    if ((passSinEspacios.Length < 8 && passSinEspacios.Length > 15) || !passSinEspacios.ToCharArray().Any(char.IsDigit))
                    {
                        UserDialogs.Instance.Alert("La contraseña debe tener como mínimo 8 caracteres y un máximo de 15, incluyendo una letra minúscula, una mayúscula y un número.", "Error", "OK");
                    }
                    else
                    {
                        try
                        {
                            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constantes.WebAPIkey));
                            var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(nombreSinEspacios, passSinEspacios,  sendVerificationEmail: true);
                            string gettoken = auth.FirebaseToken;
                            var usuario = await FirebaseHelper.InsertarUsuario(nombreSinEspacios, gettoken,  Constantes.GenerarId());
                            if (usuario)
                            {
                                UserDialogs.Instance.Alert("Se ha enviado un mensaje de confirmación a su correo electrónico.", "Antes de empezar", "OK");
                                Application.Current.MainPage = new AppShell(nombreSinEspacios);
                            }
                            
                        }
                        catch (Exception)
                        {
                            UserDialogs.Instance.Alert("Se ha producido un fallo en el registro. Por favor, compruebe su conexión a internet e inténtelo de nuevo.", "Error", "OK");
                        }
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("Debe introducir una contraseña.", "Error", "OK");
                }
            }
        }
    }
}