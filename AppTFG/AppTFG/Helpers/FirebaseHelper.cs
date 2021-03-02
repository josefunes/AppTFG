using AppTFG.Modelos;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTFG.Helpers
{
    public class FirebaseHelper
    {
        //Conexión Base de datos en tiempo real
        public static FirebaseClient firebase = new FirebaseClient("https://pruebaauth-c50d4-default-rtdb.europe-west1.firebasedatabase.app/");
        //Conexión Almacenaimento contenido multimedia
        public static FirebaseStorage firebaseStorage = new FirebaseStorage("pruebaauth-c50d4.appspot.com");

        //MÉTODOS CRUD USUARIO
        public static async Task<List<Usuario>> ObtenerTodosUsuarios()
        {
            try
            {
                var listaUsuarios = (await firebase
                .Child("Usuarios")
                .OnceAsync<Usuario>()).Select(item =>
                new Usuario
                {
                    Nombre = item.Object.Nombre,
                    Password = item.Object.Password,
                    UsuarioId = item.Object.UsuarioId
                }).ToList();
                return listaUsuarios;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Leer 
        public static async Task<Usuario> ObtenerUsuario(string nombre)
        {
            try
            {
                var todosUsuarios = await ObtenerTodosUsuarios();
                await firebase
                .Child("Usuarios")
                .OnceAsync<Usuario>();
                return todosUsuarios.Where(a => a.Nombre == nombre).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar
        public static async Task<bool> InsertarUsuario(string nombre, string password, int id)
        {
            try
            {
                await firebase
                .Child("Usuarios")
                .PostAsync(new Usuario() { Nombre = nombre, Password = password, UsuarioId = id});
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar
        public static async Task<bool> ActualizarUsuario(string nombre, string password, int id)
        {
            try
            {
                var actualizarUsuario = (await firebase
                .Child("Usuarios")
                .OnceAsync<Usuario>()).Where(a => a.Object.Nombre == nombre).FirstOrDefault();
                await firebase
                .Child("Usuarios")
                .Child(actualizarUsuario.Key)
                .PutAsync(new Usuario() { Nombre = nombre, Password = password, UsuarioId = id });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Eliminar
        public static async Task<bool> EliminarUsuario(string nombre)
        {
            try
            {
                var eliminarUsuario = (await firebase
                .Child("Usuarios")
                .OnceAsync<Usuario>()).Where(a => a.Object.Nombre == nombre).FirstOrDefault();
                await firebase.Child("Usuarios").Child(eliminarUsuario.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //MÉTODOS CRUD PUEBLO

        public static async Task<List<Pueblo>> ObtenerTodosPueblos()
        {
            try
            {
                var listaPueblos = (await firebase
                .Child("Pueblos")
                .OnceAsync<Pueblo>()).Select(item =>
                new Pueblo
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Descripcion = item.Object.Descripcion,
                    ImagenPrincipal = item.Object.ImagenPrincipal,
                    Fotos = item.Object.Fotos,
                    Videos = item.Object.Videos,
                    Rutas = item.Object.Rutas,
                    Actividades = item.Object.Actividades,
                    //IdUsuario = item.Object.IdUsuario
                }).ToList();
                return listaPueblos;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //public static async Task<Pueblo> ObtenerTodosPueblosUsuario(string nombreUsuario)
        //{
        //    try
        //    {
        //        var todosPueblos = await ObtenerTodosPueblos();
        //        await firebase.Child("Pueblos").OnceAsync<Pueblo>();
        //        return (Pueblo)todosPueblos.Where(a => a.Usuario.Nombre.Equals(nombreUsuario));
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine($"Error:{e}");
        //        return null;
        //    }
        //}

        //Leer 
        public static async Task<Pueblo> ObtenerPueblo(int id)
        {
            try
            {
                var todosPueblos = await ObtenerTodosPueblos();
                await firebase
                .Child("Pueblos")
                .OnceAsync<Pueblo>();
                return todosPueblos.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar
        public static async Task<bool> InsertarPueblo(int id, string nombre, string descrpcion, string imagen) /*, int idUsuario*/
        {
            try
            {
                await firebase
                .Child("Pueblos")
                .PostAsync(new Pueblo() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen }); /*, IdUsuario = idUsuario*/
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar 
        public static async Task<bool> ActualizarPueblo(int id, string nombre, string descrpcion, string imagen)
        {
            try
            {
                var actualizarPueblo = (await firebase
                .Child("Pueblos")
                .OnceAsync<Pueblo>()).Where(a => a.Object.Id == id).FirstOrDefault();
                //List<Foto> fotos = actualizarPueblo.Object.Fotos;
                //List<Video> videos = actualizarPueblo.Object.Videos;
                //List<Ruta> rutas = actualizarPueblo.Object.Rutas;
                //List<Actividad> actividades = actualizarPueblo.Object.Actividades;
                //fotos.Add(foto);
                //videos.Add(video);
                //rutas.Add(ruta);
                //actividades.Add(actividad);
                await firebase
                .Child("Pueblos")
                .Child(actualizarPueblo.Key)
                .PutAsync(new Pueblo() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Eliminar
        public static async Task<bool> EliminarPueblo(int id)
        {
            try
            {
                var eliminarPueblo = (await firebase
                .Child("Pueblos")
                .OnceAsync<Pueblo>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Pueblos").Child(eliminarPueblo.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //MÉTODOS CRUD RUTA

        public static async Task<List<Ruta>> ObtenerTodasRutas()
        {
            try
            {
                var listaRutas = (await firebase
                .Child("Rutas")
                .OnceAsync<Ruta>()).Select(item =>
                new Ruta
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Descripcion = item.Object.Descripcion,
                    ImagenPrincipal = item.Object.ImagenPrincipal,
                    VideoUrl = item.Object.VideoUrl,
                    Pueblo = item.Object.Pueblo
                }).ToList();
                return listaRutas;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public static async Task<List<Ruta>> ObtenerTodasRutasPueblo(string nombrePueblo)
        {
            try
            {
                var todasRutas = await ObtenerTodasRutas();
                await firebase.Child("Rutas").OnceAsync<Ruta>();
                return todasRutas.Where(a => a.Pueblo.Nombre.Equals(nombrePueblo)).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Read 
        public static async Task<Ruta> ObtenerRuta(int id)
        {
            try
            {
                var todasRutas = await ObtenerTodasRutas();
                await firebase
                .Child("Rutas")
                .OnceAsync<Ruta>();
                return todasRutas.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Inser a user
        public static async Task<bool> InsertarRuta(int id, string nombre, string descrpcion, string imagen, Video video, Pueblo pueblo)
        {
            try
            {
                await firebase
                .Child("Rutas")
                .PostAsync(new Ruta() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen, VideoUrl = video, Pueblo = pueblo });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Update 
        public static async Task<bool> ActualizarRuta(int id, string nombre, string descrpcion, string imagen, Video video)
        {
            try
            {
                var actualizarRuta = (await firebase
                .Child("Rutas")
                .OnceAsync<Ruta>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Rutas")
                .Child(actualizarRuta.Key)
                .PutAsync(new Ruta() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen, VideoUrl = video });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Delete User
        public static async Task<bool> EliminarRuta(int id)
        {
            try
            {
                var eliminarRuta = (await firebase
                .Child("Rutas")
                .OnceAsync<Ruta>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Rutas").Child(eliminarRuta.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //MÉTODOS CRUD ACTIVIDAD

        public static async Task<List<Actividad>> ObtenerTodasActividades()
        {
            try
            {
                var listaActividades = (await firebase
                .Child("Actividades")
                .OnceAsync<Actividad>()).Select(item =>
                new Actividad
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Descripcion = item.Object.Descripcion,
                    ImagenPrincipal = item.Object.ImagenPrincipal,
                    VideoUrl = item.Object.VideoUrl,
                    Pueblo = item.Object.Pueblo
                }).ToList();
                return listaActividades;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public static async Task<List<Actividad>> ObtenerTodasActividadesPueblo(string nombrePueblo)
        {
            try
            {
                var todasActividades = await ObtenerTodasActividades();
                await firebase.Child("Actividades").OnceAsync<Actividad>();
                return todasActividades.Where(a => a.Pueblo.Nombre.Equals(nombrePueblo)).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Leer 
        public static async Task<Actividad> ObtenerActividad(int id)
        {
            try
            {
                var todasActividades = await ObtenerTodasActividades();
                await firebase
                .Child("Actividades")
                .OnceAsync<Actividad>();
                return todasActividades.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar
        public static async Task<bool> InsertarActividad(int id, string nombre, string descrpcion, string imagen, Video video, Pueblo pueblo)
        {
            try
            {
                await firebase
                .Child("Actividades")
                .PostAsync(new Actividad() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen, VideoUrl = video, Pueblo = pueblo });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar
        public static async Task<bool> ActualizarActividad(int id, string nombre, string descrpcion, string imagen, Video video)
        {
            try
            {
                var actualizarActividad = (await firebase
                .Child("Actividades")
                .OnceAsync<Actividad>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Actividades")
                .Child(actualizarActividad.Key)
                .PutAsync(new Actividad() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen, VideoUrl = video });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Eliminar
        public static async Task<bool> EliminarActividad(int id)
        {
            try
            {
                var eliminarActividad = (await firebase
                .Child("Actividades")
                .OnceAsync<Actividad>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Actividades").Child(eliminarActividad.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //MÉTODOS CRUD FOTOS

        public static async Task<List<Foto>> ObtenerTodasFotos()
        {
            try
            {
                var listaFotos = (await firebase
                .Child("Fotos")
                .OnceAsync<Foto>()).Select(item =>
                new Foto
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Imagen = item.Object.Imagen,
                    Pueblo = item.Object.Pueblo
                }).ToList();
                return listaFotos;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public static async Task<List<Foto>> ObtenerTodasFotosPueblo(string nombrePueblo)
        {
            try
            {
                var todasFotos = await ObtenerTodasFotos();
                await firebase.Child("Fotos").OnceAsync<Foto>();
                return todasFotos.Where(a => a.Pueblo.Nombre.Equals(nombrePueblo)).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Leer 
        public static async Task<Foto> ObtenerFoto(int id)
        {
            try
            {
                var todasFotos = await ObtenerTodasFotos();
                await firebase
                .Child("Fotos")
                .OnceAsync<Foto>();
                return todasFotos.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar foto
        public static async Task<bool> InsertarFoto(int id, string nombre, string imagen, Pueblo pueblo)
        {
            try
            {
                await firebase
                .Child("Fotos")
                .PostAsync(new Foto() { Id = id, Nombre = nombre, Imagen = await CargarFoto(imagen), Pueblo = pueblo });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar
        public static async Task<bool> ActualizarFoto(int id, string nombre, string imagen)
        {
            try
            {
                var actualizarFoto = (await firebase
                .Child("Fotos")
                .OnceAsync<Foto>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Fotos")
                .Child(actualizarFoto.Key)
                .PutAsync(new Foto() { Id = id, Nombre = nombre, Imagen = imagen});
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Eliminar
        public static async Task<bool> EliminarFoto(int id)
        {
            try
            {
                var eliminarFoto = (await firebase
                .Child("Fotos")
                .OnceAsync<Foto>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Fotos").Child(eliminarFoto.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        public static async Task<string> SubirFoto(Stream fileStream, string fileName)
        {
            var imageUrl = await firebaseStorage
                .Child("Fotos")
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }

        public static async Task<string> CargarFoto(string fileName)
        {
            return await firebaseStorage
                .Child("Fotos")
                .Child(fileName)
                .GetDownloadUrlAsync();
        }

        public static async Task BorrarFoto(string fileName)
        {
            await firebaseStorage
                 .Child("Fotos")
                 .Child(fileName)
                 .DeleteAsync();
        }

        //MÉTODOS CRUD VIDEOS

        public static async Task<List<Video>> ObtenerTodosVideos()
        {
            try
            {
                var listaVideos = (await firebase
                .Child("Videos")
                .OnceAsync<Video>()).Select(item =>
                new Video
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Videoclip = item.Object.Videoclip,
                    Pueblo = item.Object.Pueblo
                }).ToList();
                return listaVideos;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public static async Task<List<Video>> ObtenerTodosVideosPueblo(string nombrePueblo)
        {
            try
            {
                var todasVideos = await ObtenerTodosVideos();
                await firebase.Child("Videos").OnceAsync<Video>();
                return todasVideos.Where(a => a.Pueblo.Nombre.Equals(nombrePueblo)).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Leer 
        public static async Task<Video> ObtenerVideo(int id)
        {
            try
            {
                var todosVideos = await ObtenerTodosVideos();
                await firebase
                .Child("Videos")
                .OnceAsync<Video>();
                return todosVideos.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar video
        public static async Task<bool> InsertarVideo(int id, string nombre, string videoclip, Pueblo pueblo)
        {
            try
            {
                await firebase
                .Child("Videos")
                .PostAsync(new Video() { Id = id, Nombre = nombre, Videoclip = await CargarVideo(videoclip), Pueblo = pueblo });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar
        public static async Task<bool> ActualizarVideo(int id, string nombre, string videoclip)
        {
            try
            {
                var actualizarVideo = (await firebase
                .Child("Videos")
                .OnceAsync<Video>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Videos")
                .Child(actualizarVideo.Key)
                .PutAsync(new Video() { Id = id, Nombre = nombre, Videoclip = videoclip});
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Eliminar
        public static async Task<bool> EliminarVideo(int id)
        {
            try
            {
                var eliminarVideo = (await firebase
                .Child("Videos")
                .OnceAsync<Video>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Videos").Child(eliminarVideo.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //public static async Task<string> SubirVideo(Stream fileStream, string fileName)
        //{
        //    var imageUrl = await firebaseStorage
        //        .Child("Videos")
        //        .Child(fileName)
        //        .PutAsync(fileStream);
        //    return imageUrl;
        //}

        public static async Task<string> CargarVideo(string fileName)
        {
            return await firebaseStorage
                .Child("Videos")
                .Child(fileName)
                .GetDownloadUrlAsync();
        }

        //public static async Task BorrarVideo(string fileName)
        //{
        //    await firebaseStorage
        //         .Child("Videos")
        //         .Child(fileName)
        //         .DeleteAsync();
        //}
    }
}