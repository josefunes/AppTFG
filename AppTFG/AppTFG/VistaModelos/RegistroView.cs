using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Paginas;
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
                        UserDialogs.Instance.Alert("", "La contraseña introducida tiene que coincidir con la anterior", "OK");
                });
            }
        }
        private async void SignUp()
        {
            //null or empty field validation, check weather email and password is null or empty

            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Password))
                UserDialogs.Instance.Alert("Campos vacíos", "Por favor, introduzca un nombre de usuario y una contraseña", "OK");
            else if (!string.IsNullOrEmpty(Nombre))
            {
                var user = await FirebaseHelper.ObtenerUsuario(Nombre);
                if (user != null)
                { 
                    if (Nombre == user.Nombre)
                    {
                        UserDialogs.Instance.Alert("Usuario existente", "Por favor, introduzca un nombre de usuario distinto", "OK");
                    }
                }
                else if (!string.IsNullOrEmpty(Password))
                {
                    if ((Password.Length < 8 && Password.Length > 15) || !Password.ToCharArray().Any(char.IsDigit))
                    {
                        UserDialogs.Instance.Alert("Error", "La contraseña debe tener como mínimo 8 caracteres y un máximo de 15, incluyendo una letra minúscula, una mayúscula y un número", "OK");
                    }
                    else
                    {
                        var pass = Constantes.Cifrar(Password);
                        //var id = Constantes.GenerarId();
                        //call AddUser function which we define in Firebase helper class
                        var usuario = await FirebaseHelper.InsertarUsuario(Nombre, pass, 0);
                        //AddUser return true if data insert successfuly 
                        if (usuario)
                        {
                            //await Application.Current.MainPage.DisplayAlert("SignUp Success", "", "Ok");
                            //Navigate to Wellcom page after successfuly SignUp
                            //pass user email to welcome page
                            Application.Current.MainPage = new AppShell(Nombre);
                        }
                        else
                        {
                            UserDialogs.Instance.Alert("Error", "Fallo en el Registro", "OK");
                        }
                    }
                } 
            }
        }
    }
}