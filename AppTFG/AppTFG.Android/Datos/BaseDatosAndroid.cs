using System;
using System.IO;
using AppTFG.Datos;
using AppTFG.Droid.Datos;
using AppTFG.Helpers;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseDatosAndroid))]
namespace AppTFG.Droid.Datos
{
    public class BaseDatosAndroid : IBaseDatos
    {
  //      public SQLiteConnection GetConnection()
  //      {
		//	var sqliteFilename = Constantes.NombreBD;
		//	string documentsPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
		//	var path = Path.Combine(documentsPath, sqliteFilename);

		//	// This is where we copy in the prepopulated database
		//	Console.WriteLine(path);
		//	if (!File.Exists(path))
		//	{
		//		var s = Forms.Context.Resources.OpenRawResource(Resource.Raw.BD_SistemaRural);  // RESOURCE NAME ###

		//		// create a write stream
		//		FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
		//		// write to the stream
		//		ReadWriteStream(s, writeStream);
		//	}

		//	//var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
		//	var conn = new SQLiteConnection(path);

		//	// Return the database connection 
		//	return conn;
		//}

  //      void ReadWriteStream(Stream readStream, Stream writeStream)
  //      {
  //          int Length = 256;
  //          byte[] buffer = new byte[Length];
  //          int bytesRead = readStream.Read(buffer, 0, Length);
  //          // write the required bytes
  //          while (bytesRead > 0)
  //          {
  //              writeStream.Write(buffer, 0, bytesRead);
  //              bytesRead = readStream.Read(buffer, 0, Length);
  //          }
  //          readStream.Close();
  //          writeStream.Close();
  //      }

        public string GetDatabasePath()
        {
            var filename = Constantes.NombreBD;
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, filename);
            //var connection = new SQLiteConnection(path);
            return path;
        }
    }
}