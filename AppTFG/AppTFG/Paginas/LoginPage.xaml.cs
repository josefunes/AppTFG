﻿using Acr.UserDialogs;
using AppTFG.VistaModelos;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                UserDialogs.Instance.Alert("Sin conexión a internet no es posible usar la app. Conéctate a una red y vuelve a intentarlo.", "Fallo al iniciar sesión", "OK");
            }
            InitializeComponent();
            //Task.Run(AnimateBackground);
            BindingContext = new LoginView();
        }


        //private async void AnimateBackground()
        //{
        //    void forward(double input) => bdGradient.AnchorY = input;
        //    void backward(double input) => bdGradient.AnchorY = input;

        //    while (true)
        //    {
        //        bdGradient.Animate(name: "forward", callback: forward, start: 0, end: 1, length: 5000, easing: Easing.SinIn);
        //        await Task.Delay(5000);
        //        bdGradient.Animate(name: "backward", callback: backward, start: 1, end: 0, length: 5000, easing: Easing.SinIn);
        //        await Task.Delay(5000);
        //    }
        //}
    }
}