using System;
using AppTFG.Helpers;
using System.Diagnostics;
using System.ComponentModel;
using Xamarin.Forms;
using System.Linq;
using Acr.UserDialogs;
using Firebase.Auth;
using AppTFG.Paginas;
using Xamarin.Essentials;
using Newtonsoft.Json;

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
                    {
                        Update();
                        UserDialogs.Instance.Alert("La contraseña ha sido actualizada.", "", "OK");
                    }
                    else
                        UserDialogs.Instance.Alert("La contraseña introducida tiene que coincidir con la anterior.", "", "OK");
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
                nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.Inicio));
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
                        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constantes.WebAPIkey));
                        try
                        {
                            //This is the saved firebaseauthentication that was saved during the time of login
                            var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken",""));
                            //Here we are Refreshing the token
                            var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                            Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
                            //Now lets grab user information
                            string token = savedfirebaseauth.FirebaseToken;
                            var nuevo = await authProvider.ChangeUserPassword(token, Password);
                            var auth = await authProvider.SignInWithEmailAndPasswordAsync(Nombre, Password);
                            var isupdate = await FirebaseHelper.ActualizarUsuario(nombre, usuario.UsuarioId);
                            if (isupdate)
                                UserDialogs.Instance.Alert("", "Contraseña actualizada", "Ok");
                            else
                                UserDialogs.Instance.Alert("No se ha podido actualizar.", "Error", "Ok");
                        }
                        catch (Exception)
                        {
                            UserDialogs.Instance.Alert("Por favor, introduzca un nombre de usuario y una contraseña correctos", "Fallo al iniciar sesión", "OK");
                        }
                    }
                }
                else
                    UserDialogs.Instance.Alert("Por favor, introduzca una nueva contraseña.", "Inserte una contraseña válida", "Ok");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error catch:{e}");
            }
        }

        //Delete user data
        private async void Delete()
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.Inicio));
            string nombre = nombreUsuario.Text;
            var usuario = await FirebaseHelper.ObtenerUsuario(nombre);
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constantes.WebAPIkey));
            try
            {
                var eliminar = await UserDialogs.Instance.ConfirmAsync("Si desea que se eliminen todos los datos asociados" +
                    "al pueblo, tendrá que eliminarlo en la pantalla 'Mi pueblo'.", "Advertencia", "Eliminar usuario", "Cancelar" );
                if (eliminar)
                {
                    await authProvider.DeleteUserAsync(usuario.FirebaseToken);
                    var isdelete = await FirebaseHelper.EliminarUsuario(nombre);
                    if (isdelete)
                        await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                    else
                        UserDialogs.Instance.Alert("No se ha podido eliminar el usuario", "Error", "Ok");
                }
            }
            catch (Exception e)
            {
                //UserDialogs.Instance.Alert("No se ha podido eliminar el usuario", "", "Ok");
                Debug.WriteLine($"Error:{e}");
            }
        }
    }
}
