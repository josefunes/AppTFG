using AppTFG.Helpers;
using AppTFG.Paginas;
using System.ComponentModel;
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
        public Command SignUpCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Password == ConfirmPassword)
                        SignUp();
                    else
                        Application.Current.MainPage.DisplayAlert("", "La contraseña introducida tiene que coincidir con la anterior", "OK");
                });
            }
        }
        private async void SignUp()
        {
            //null or empty field validation, check weather email and password is null or empty

            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Password))
                await Application.Current.MainPage.DisplayAlert("Campos vacíos", "Por favor, introduzca un nombre de usuario y una contraseña", "OK");
            else
            {
                //call AddUser function which we define in Firebase helper class
                var user = await FirebaseHelper.InsertarUsuario(Nombre, Password, Constantes.GenerarId());
                //AddUser return true if data insert successfuly 
                if (user)
                {
                    //await Application.Current.MainPage.DisplayAlert("SignUp Success", "", "Ok");
                    //Navigate to Wellcom page after successfuly SignUp
                    //pass user email to welcome page
                    await App.Current.MainPage.Navigation.PushAsync(new InicioPage(Nombre));
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Fallo en el Registro", "OK");

            }
        }
    }
}
