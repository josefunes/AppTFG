using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubirAudio : ContentPage
    {
        private Audio Audio;
        AudioRecorderService recorder;
        AudioPlayer player;
        bool isTimerRunning = false;
        int seconds = 0, minutes = 0;
        public SubirAudio(Audio audio)
        {
            Audio = audio;
            BindingContext = audio;
            InitializeComponent();
            recorder = new AudioRecorderService();
            player = new AudioPlayer();
            //Este evento se lanza cuando se termine la grabación
            player.FinishedPlaying += Finish_Playing;
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        void Finish_Playing(object sender, EventArgs e)
        {
            bntRecord.IsEnabled = true;
            bntRecord.BackgroundColor = Color.FromHex("#7cbb45");
            bntPlay.IsEnabled = true;
            bntPlay.BackgroundColor = Color.FromHex("#7cbb45");
            bntStop.IsEnabled = false;
            bntStop.BackgroundColor = Color.Silver;
            lblSeconds.Text = "00";
            lblMinutes.Text = "00";
        }

        async void Record_Clicked(object sender, EventArgs e)
        {
            if (!recorder.IsRecording)
            {
                seconds = 0;
                minutes = 0;
                isTimerRunning = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                    seconds++;

                    if (seconds.ToString().Length == 1)
                    {
                        lblSeconds.Text = "0" + seconds.ToString();
                    }
                    else
                    {
                        lblSeconds.Text = seconds.ToString();
                    }
                    if (seconds == 60)
                    {
                        minutes++;
                        seconds = 0;

                        if (minutes.ToString().Length == 1)
                        {
                            lblMinutes.Text = "0" + minutes.ToString();
                        }
                        else
                        {
                            lblMinutes.Text = minutes.ToString();
                        }

                        lblSeconds.Text = "00";
                    }
                    return isTimerRunning;
                });
                recorder.FilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".wav";
                var audioRecordTask = await recorder.StartRecording();

                bntRecord.IsEnabled = false;
                bntRecord.BackgroundColor = Color.Silver;
                bntPlay.IsEnabled = false;
                bntPlay.BackgroundColor = Color.Silver;
                bntStop.IsEnabled = true;
                bntStop.BackgroundColor = Color.FromHex("#7cbb45");
                
                await audioRecordTask;
            }
        }

        async void Stop_Clicked(object sender, EventArgs e)
        {
            StopRecording();
            await recorder.StopRecording();
        }

        void StopRecording()
        {
            isTimerRunning = false;
            bntRecord.IsEnabled = true;
            bntRecord.BackgroundColor = Color.FromHex("#7cbb45");
            bntPlay.IsEnabled = true;
            bntPlay.BackgroundColor = Color.FromHex("#7cbb45");
            bntStop.IsEnabled = false;
            bntStop.BackgroundColor = Color.Silver;
            lblSeconds.Text = "00";
            lblMinutes.Text = "00";
        }
        void Play_Clicked(object sender, EventArgs e)
        {
            try
            {
                //Esta instrucción sirve para dejar de reproducir un audio cuando se quiere reproducir otro
                player.Pause();
                var filePath1 = recorder.GetAudioFilePath();
                var filePath2 = recorder.FilePath;
                if (filePath1 != null)
                {
                    StopRecording();
                    player.Play(filePath1);
                    bntPlay.BackgroundColor = Color.FromHex("#1ae579");
                }
                else if (filePath2 != null)
                {
                    StopRecording();
                    player.Play(filePath2);
                    bntPlay.BackgroundColor = Color.FromHex("#1ae579");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void BtnGuardarExp_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var audio = (Audio)BindingContext;
            var ruta = await FirebaseHelper.ObtenerRuta(audio.IdRuta);
            //Pongo esta condición porque da un error (Null) que no logro controlar si no se pone
            if(txtNombre.Text == null)
            {
                txtNombre.Text = "";
            }
            if (audio.Id > 0)
            {
                if (txtNombre.Text.Equals("") || txtNombre == null || string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    UserDialogs.Instance.Alert("Es necesario introducir el nombre del audio/descripción correspondiente con la ubicación.", "Error", "OK");
                    return;
                }
                if (txtNumero.Text.Equals("") || txtNumero == null || string.IsNullOrWhiteSpace(txtNumero.Text))
                {
                    UserDialogs.Instance.Alert("Es necesario introducir el número del audio/descripción correspondiente con la ubicación.", "Error", "OK");
                    return;
                }
                if (txtNumero.Text.ToCharArray().All(char.IsDigit) == false)
                {
                    UserDialogs.Instance.Alert("Es necesario introducir un número para identificar al Audio.", "Error", "OK");
                    return;
                }
                else
                {
                    foreach(var audio1 in ruta.Audios)
                    {
                        if(audio1.Id == audio.Id)
                        {
                            audio1.Numero = int.Parse(txtNumero.Text);
                            audio1.Nombre = txtNombre.Text;
                            audio1.Descripcion = txtDescripcion.Text;
                            if (!string.IsNullOrEmpty(recorder.FilePath) || !string.IsNullOrEmpty(recorder.GetAudioFilePath()))
                            {
                                bool answer = await UserDialogs.Instance.ConfirmAsync("Hay información grabada con el micrófono. ¿Desea guardarla?", "Atención", "Sí", "No");
                                if (answer == true)
                                {
                                    audio1.Sonido = await FirebaseHelper.SubirAudio(recorder.GetAudioFileStream(), audio1.Nombre);
                                }
                            }
                            await FirebaseHelper.ActualizarRuta(ruta.Id, ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones, ruta.Audios, ruta.Valoraciones);
                        }
                        break;
                    }
                }
            }
            else
            {
                if (ruta.Audios == null)
                {
                    ruta.Audios = new List<Audio>();
                }
                if (txtNombre.Text.Equals("") || string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrEmpty(txtNombre.Text))
                {
                    UserDialogs.Instance.Alert("Es necesario introducir el nombre del audio/descripción correspondiente con la ubicación.", "Error", "OK");
                    return;
                }
                if (txtNumero.Text.Equals("") || txtNumero.Text == null || string.IsNullOrWhiteSpace(txtNumero.Text))
                {
                    UserDialogs.Instance.Alert("Es necesario introducir el número del audio/descripción correspondiente con la ubicación.", "Error", "OK");
                    return;
                }
                foreach (var audio1 in ruta.Audios)
                {
                    if (audio1.Numero.ToString().Equals(txtNumero.Text.ToString()))
                    {
                        UserDialogs.Instance.Alert("El número del audio/descripción pertenece a un registro ya guardado. Comprueba qué número deseas introducir.", "Error", "OK");
                        return;
                    }
                    break;
                }
                if (txtNumero.Text.ToCharArray().All(char.IsDigit) == false)
                {
                    UserDialogs.Instance.Alert("Es necesario introducir un número para identificar al audio/descripción.", "Error", "OK");
                    return;
                }
                else
                {
                    var audioNuevo = new Audio
                    {
                        Id = Constantes.GenerarId(),
                        IdRuta = ruta.Id,
                        Numero = int.Parse(txtNumero.Text),
                        Nombre = txtNombre.Text,
                        Descripcion = txtDescripcion.Text
                    };
                    if (!string.IsNullOrEmpty(recorder.FilePath) || !string.IsNullOrEmpty(recorder.GetAudioFilePath()))
                    {
                        bool answer = await UserDialogs.Instance.ConfirmAsync("Hay información grabada con el micrófono. ¿Desea guardarla?", "Atención", "Sí", "No");
                        if (answer == true)
                        {
                            audioNuevo.Sonido = await FirebaseHelper.SubirAudio(recorder.GetAudioFileStream(), audioNuevo.Nombre);
                        }
                    }
                    if (audioNuevo.Numero.Equals(""))
                    {
                        UserDialogs.Instance.Alert("Es necesario introducir el número del audio correspondiente con la ubicación.", "Error", "OK");
                        return;
                    }
                    ruta.Audios.Add(audioNuevo);
                    await FirebaseHelper.ActualizarRuta(ruta.Id, ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones, ruta.Audios, ruta.Valoraciones);
                }
            }
            Loading(false);
            UserDialogs.Instance.Alert("Registro realizado correctamente.", "Correcto", "OK");
            await Navigation.PopAsync();
        }

        private async void BtnBorrarExp_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading(true);
                var audio = (Audio)BindingContext;
                var ruta = await FirebaseHelper.ObtenerRuta(audio.IdRuta);
                for (int pos = 0; pos < ruta.Audios.Count; pos++)
                {
                    if (ruta.Audios[pos].Id.Equals(audio.Id))
                    {
                        ruta.Audios.RemoveAt(pos);
                        break;
                    }
                }
                await FirebaseHelper.ActualizarRuta(ruta.Id, ruta.Nombre, ruta.Descripcion, ruta.ImagenPrincipal, ruta.VideoUrl, ruta.IdPueblo, ruta.Camino, ruta.Ubicaciones, ruta.Audios, ruta.Valoraciones);
                Loading(false);
                UserDialogs.Instance.Alert("Explicación eliminada correctamente", "Correcto", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}