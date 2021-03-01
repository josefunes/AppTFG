using AppTFG.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AppTFG.VistaModelos
{
    public class InicioView
    {
        public InicioView(string nombre2)
        {
            Nombre = nombre2;
        }
        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
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

        //public Command UpdateCommand
        //{
        //    get { return new Command(Update); }
        //}

        public Command DeleteCommand
        {
            get { return new Command(Delete); }
        }
        //For Logout
        public Command LogoutCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.Current.MainPage.Navigation.PopAsync();
                });
            }
        }
        //Update user data
        //private async void Update()
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(Password))
        //        {
        //            var isupdate = await FirebaseHelper.ActualizarUsuario(Nombre, Password);
        //            if (isupdate)
        //                await App.Current.MainPage.DisplayAlert("Contraseña actualizada", "", "Ok");
        //            else
        //                await App.Current.MainPage.DisplayAlert("Error", "No se ha podido actualizar", "Ok");
        //        }
        //        else
        //            await App.Current.MainPage.DisplayAlert("Inserte una contraseña válida", "Por favor, introduzca una nueva contraseña", "Ok");
        //    }
        //    catch (Exception e)
        //    {

        //        Debug.WriteLine($"Error:{e}");
        //    }
        //}
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
