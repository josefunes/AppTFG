using System;
using AppTFG.Helpers;
using System.Diagnostics;
using System.ComponentModel;
using Xamarin.Forms;
using System.Linq;

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
                        Application.Current.MainPage.DisplayAlert("", "La contraseña introducida tiene que coincidir con la anterior", "OK");
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
                        await Application.Current.MainPage.DisplayAlert("Error", "Contraseña actual errónea. Pruebe de nuevo.", "OK");
                    }
                    else if ((NewPassword.Length < 8 && NewPassword.Length > 15) || !NewPassword.ToCharArray().Any(Char.IsDigit))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "La contraseña debe tener como mínimo 8 caracteres y un máximo de 15, incluyendo una letra minúscula, una mayúscula y un número.", "OK");
                    }
                    else
                    {
                        var pass = Constantes.Cifrar(NewPassword);
                        //call AddUser function which we define in Firebase helper class
                        var isupdate = await FirebaseHelper.ActualizarUsuario(nombre, pass, usuario.UsuarioId);
                        if (isupdate)
                            await App.Current.MainPage.DisplayAlert("Contraseña actualizada", "", "Ok");
                        else
                            await App.Current.MainPage.DisplayAlert("Error", "No se ha podido actualizar.", "Ok");
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert("Inserte una contraseña válida", "Por favor, introduzca una nueva contraseña.", "Ok");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
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
                    await App.Current.MainPage.DisplayAlert("Error", "No se ha podido eliminar el usuario", "Ok");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
            }
        }
    }
}
