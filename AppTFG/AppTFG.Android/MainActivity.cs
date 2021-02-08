using Android.App;
using Android.Runtime;
using Android.OS;
using System.Threading.Tasks;
using Android.Content;
using Android;
using System;
using Android.Content.PM;
//using System.IO;
//using AppTFG.Helpers;
//using SQLite;
//using Microsoft.Data.Sqlite;
//using Environment = System.Environment;

namespace AppTFG.Droid
{
    [Activity(Label = "AppTFG")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Current = this;
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);


            //var sqliteFilename = Constantes.NombreBD;
            //string documentsPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            //var path = Path.Combine(documentsPath, sqliteFilename);
            //using (SqliteConnection db = new SqliteConnection("Filename=" + documentsPath))
            //{
            //    db.Open();
            //}
            //This is where we copy in the prepopulated database
            //    Console.WriteLine(path);
            //if (!File.Exists(path))
            //{
            //    var s = Resources.OpenRawResource(Resource.Raw.BD_SistemaRural);  // RESOURCE NAME ###

            //    create a write stream
            //    FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            //    write to the stream
            //    ReadWriteStream(s, writeStream);
            //}
            //var plat = new Platform.XamarinAndroid.SQLitePlatformAndroid();
            //var conn = new SQLiteAsyncConnection(path);

            //Set the database connection string
            //App.SetDatabaseConnection(conn);

            LoadApplication(new App());
        }

        public static MainActivity Current { private set; get; }

        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<string> PickImageTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    // Set the filename as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(data.DataString);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    Console.WriteLine("Location permissions already granted.");
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == RequestLocationId)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
                {
                    Console.WriteLine("Location permissions granted.");
                }
                else
                {
                    Console.WriteLine("Location permissions denied.");
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }

            //Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //void ReadWriteStream(Stream readStream, Stream writeStream)
        //{
        //    int Length = 256;
        //    byte[] buffer = new byte[Length];
        //    int bytesRead = readStream.Read(buffer, 0, Length);
        //    // write the required bytes
        //    while (bytesRead > 0)
        //    {
        //        writeStream.Write(buffer, 0, bytesRead);
        //        bytesRead = readStream.Read(buffer, 0, Length);
        //    }
        //    readStream.Close();
        //    writeStream.Close();
        //}

    }
}