using System;
using AppTFG.Helpers;
using System.Diagnostics;
using System.ComponentModel;
using Xamarin.Forms;
using System.Linq;
using Acr.UserDialogs;

namespace AppTFG.VistaModelos
{
    public class AjustesView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        private string newPassword;

        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NewPassword"));
            }
        }

        public Command UpdateCommand
        {
            get 
            {
                return new Command(() =>
                {
                    if (Password == ConfirmPassword)
                        Update();
                    else
                        UserDialogs.Instance.Alert("La contraseña introducida tiene que coincidir con la anterior", "", "OK");
                });
            }
        }

        public Command DeleteCommand
        {
            get { return new Command(Delete); }
        }

        //Update user data
        private async void Update()
        {
            try
            {
                Label nombreUsuario = new Label();
                nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.inicio));
                string nombre = nombreUsuario.Text;
                var usuario = await FirebaseHelper.ObtenerUsuario(nombre);
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    if (!(Constantes.Descifrar(usuario.Password) == Password) || !(Constantes.Descifrar(usuario.Password) == ConfirmPassword))
                    {
                        UserDialogs.Instance.Alert("Contraseña actual errónea. Pruebe de nuevo.", "Error", "OK");
                    }
                    else if ((NewPassword.Length < 8 && NewPassword.Length > 15) || !NewPassword.ToCharArray().Any(Char.IsDigit))
                    {
                        UserDialogs.Instance.Alert("La contraseña debe tener como mínimo 8 caracteres y un máximo de 15, incluyendo una letra minúscula, una mayúscula y un número.", "Error", "OK");
                    }
                    else
                    {
                        var pass = Constantes.Cifrar(NewPassword);
                        //call AddUser function which we define in Firebase helper class
                        var isupdate = await FirebaseHelper.ActualizarUsuario(nombre, pass, usuario.UsuarioId);
                        if (isupdate)
                            UserDialogs.Instance.Alert("", "Contraseña actualizada", "Ok");
                        else
                            UserDialogs.Instance.Alert("No se ha podido actualizar.", "Error", "Ok");
                    }
                }
                else
                    UserDialogs.Instance.Alert("Por favor, introduzca una nueva contraseña.", "Inserte una contraseña válida", "Ok");
            }
            catch (Exception e)
            {
                //UserDialogs.Instance.Alert("No se ha podido actualizar la contraseña.", "", "Ok");
                Debug.WriteLine($"Error catch:{e}");
            }
        }

        //Delete user data
        private async void Delete()
        {
            try
            {
                var isdelete = await FirebaseHelper.EliminarUsuario(Nombre);
                if (isdelete)
                    await App.Current.MainPage.Navigation.PopAsync();
                else
                    UserDialogs.Instance.Alert("No se ha podido eliminar el usuario", "Error", "Ok");
            }
            catch (Exception e)
            {
                //UserDialogs.Instance.Alert("No se ha podido eliminar el usuario", "", "Ok");
                Debug.WriteLine($"Error:{e}");
            }
        }
    }
}
