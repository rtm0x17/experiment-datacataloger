using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.VisualBasic;
using NAudio.Wave;
using rtm0x17.DefectCataloger.Windows;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

using Prometheus;
using System.Diagnostics;

namespace rtm0x17.DefectCataloger
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Models.Defect> Defects { get; set; } = new ObservableCollection<Models.Defect>();

        private bool _isSessionActive = false;
        private string _lotCode;
        private Button _currentRecorderButton;
        private WaveFileWriter _recorderFileWriter;
        private WaveIn _recorder;
        private string _tempFile;
        private bool _isRecording = false;
        private TakePictureDialog _takePictureDialog = new TakePictureDialog();
        private readonly KestrelMetricServer server = new KestrelMetricServer(port: 5000);
        private System.Collections.Generic.Dictionary<string, Counter> _defectSession = new System.Collections.Generic.Dictionary<string, Counter>();

        public MainWindow()
        {
            InitializeComponent();
            server.Start();
            SetDefectsButtonsAndCounters();
            SetDataGrid();
        }

        private void SetDataGrid()
        {
            DataGridCurrentSession.DataContext = Defects;
            DataGridCurrentSession.ItemsSource = Defects;
            Defects.CollectionChanged += Defects_CollectionChanged;
        }

        private void Defects_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (DataGridCurrentSession.Items.Count > 0)
                if (VisualTreeHelper.GetChild(DataGridCurrentSession, 0) is Decorator border)
                    if (border.Child is ScrollViewer scroll)
                        scroll.ScrollToEnd();
        }

        private void SetDefectsButtonsAndCounters()
        {
            var counter = 0;
            var brashConverter = new BrushConverter();

            foreach (var defectType in Properties.Settings.Default.DefectTypes)
            {
                if (defectType is null)
                    continue;

                var button = new Button()
                {
                    Content = defectType.ToUpper(),
                    IsEnabled = false,
                    Background = (Brush)brashConverter.ConvertFrom(Properties.Settings.Default.DefectTypeColors[counter])
                };

                button.Click += AddDefect_Click;

                if (!_defectSession.ContainsKey(defectType.ToUpper()))
                    _defectSession[defectType.ToUpper()] = Metrics.CreateCounter($"current_session_defect_type_couter_{defectType.ToLower().Replace(" ", "")}", "");

                WrapPanelDefectButtons.Children.Add(button);

                counter++;

                if (counter > Properties.Settings.Default.DefectTypes.Count)
                    counter = 0;
            }
        }

        private void AddDefect_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var defectType = btn.Content.ToString();
            Defects.Add(new Models.Defect(defectType));
            _defectSession[defectType.ToUpper()].Inc();
        }

        private void SetNote_Click(object sender, RoutedEventArgs e)
        {
            string input = Interaction.InputBox("Nota:");

            if (string.IsNullOrEmpty(input))
                return;

            var btn = sender as Button;
            var guid = new Guid(btn.Tag.ToString());

            var defect = Defects.Where(q => q.Guid == guid).FirstOrDefault();

            if (defect == null)
                return;

            defect.Note = input;
        }

        private void RecordAudioNote_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null || sender is not Button) return;

            _currentRecorderButton = sender as Button;
            var recordingMessage = "Sto ascoltato ⏺️";

            if (_currentRecorderButton.Content == recordingMessage)
            {
                _recorder.StopRecording();
                return;
            }

            if (_isRecording == true)
                return;

            _currentRecorderButton.Content = recordingMessage;

            _recorder = new WaveIn();
            _recorder.WaveFormat = new WaveFormat(44100, 16, 1);

            _tempFile = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());
            _recorderFileWriter = new WaveFileWriter(_tempFile, _recorder.WaveFormat);

            _recorder.DataAvailable += (s, e) =>
            {
                _recorderFileWriter.Write(e.Buffer, 0, e.BytesRecorded);

                if (_recorderFileWriter.Position > _recorderFileWriter.WaveFormat.AverageBytesPerSecond * 30)
                    _recorder.StopRecording();
            };

            _recorder.RecordingStopped += (s, e) =>
            {
                _recorderFileWriter.Close();
                _recorderFileWriter.Dispose();

                var btn = _currentRecorderButton;
                var guid = new Guid(btn.Tag.ToString());

                var defect = Defects.Where(q => q.Guid == guid).FirstOrDefault();

                if (defect == null)
                    return;

                defect.AudioNote = File.ReadAllBytes(_tempFile);

                _recorder.Dispose();
                _isRecording = false;
            };

            _recorder.StartRecording();
            _isRecording = true;
        }

        private void PlayAudioNote_Click(object sender, RoutedEventArgs e)
        {
            var currentButton = sender as Button;
            var guid = new Guid(currentButton.Tag.ToString());

            var defect = Defects.Where(q => q.Guid == guid).FirstOrDefault();

            if (defect == null || defect.AudioNote is null)
                return;

            _tempFile = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());
            File.WriteAllBytes(_tempFile, defect.AudioNote);

            System.Media.SoundPlayer player = new(_tempFile);
            player.Play();
        }

        private void TakePicture_Click(object sender, RoutedEventArgs e)
        {
            _takePictureDialog.ShowDialog();

            if (_takePictureDialog.Image.Length > 0)
            {
                var currentButton = sender as Button;
                var guid = new Guid(currentButton.Tag.ToString());

                var defect = Defects.Where(q => q.Guid == guid).FirstOrDefault();

                if (defect == null) return;

                defect.Photo = _takePictureDialog.Image;
            }
        }

        private void ButtonSelectLot_Click(object sender, RoutedEventArgs e)
        {
            if (_isSessionActive)
            {
                if (MessageBox.Show("Vuoi terminare la sessione di controllo?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // TODO: Check if upload has been completed, if not create a local cache for next updaload

                    _lotCode = string.Empty;
                    Defects.Clear();

                    foreach (var button in WrapPanelDefectButtons.Children)
                        (button as Button).IsEnabled = false;

                    foreach (var couter in _defectSession)
                        couter.Value.IncTo(0);

                    SetSessiontStatusTo(false);

                }

                return;
            }

            var inputTextDialog = new InputTextDialog("Inseirisci il codice lotto per questa sciolta:", false);
            inputTextDialog.ShowDialog();

            _lotCode = inputTextDialog.UserInput;

            foreach (var button in WrapPanelDefectButtons.Children)
                (button as Button).IsEnabled = true;



            SetSessiontStatusTo(true);
        }

        private void SetSessiontStatusTo(bool status)
        {
            _isSessionActive = status;

            if (_isSessionActive == true)
            {
                ButtonSelectLot.Content = "CONCLUDI SESSIONE";
                ButtonShutdownComputer.Visibility = Visibility.Collapsed;
                return;
            }
            
            ButtonSelectLot.Content = "DEFINISCI LOTTO";
            ButtonShutdownComputer.Visibility = Visibility.Collapsed;
        }

        private void ButtonShutdownComputer_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Sei sicuro che vuoi spegnere il computer?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Process.Start("shutdown", "/s /t 0");
        }
    }
}
