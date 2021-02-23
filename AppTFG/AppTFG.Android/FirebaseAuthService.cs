using Android.App;
using Android.Content;
using AppTFG.Servicios;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppTFG.Droid
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        public static int REQ_AUTH = 9999;
        public static string KEY_AUTH = "auth";

        public bool IsUserSigned()
        {
            var user = Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CurrentUser;
            var signedIn = user != null;
            return signedIn;
        }

        public async Task SignUp(string email, string password)
        {
            try
            {
                await Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CreateUserWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SignIn(string email, string password)
        {
            try
            {
                await Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).SignInWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SignInWithGoogle()
        {
            var google = new Intent(Forms.Context, typeof(GoogleLoginActivity));
            ((Activity)Forms.Context).StartActivityForResult(google, REQ_AUTH);
        }
        public async Task SignInWithGoogle(string token)
        {
            try
            {
                AuthCredential credential = GoogleAuthProvider.GetCredential

                (token, null);
                await Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app)

                .SignInWithCredentialAsync(credential);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task Logout()
        {
            try
            {
                Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).SignOut();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string getAuthKey()
        {
            return KEY_AUTH;
        }

        public string GetUserId()
        {
            var user = Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CurrentUser;
            return user.Uid;
        }
    }
}
