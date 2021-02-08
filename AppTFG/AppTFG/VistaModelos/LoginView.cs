using Xamarin.Forms;

namespace AppTFG.VistaModelos
{
    public class LoginView : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginView()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync("//AppShell");
        }
    }
}
