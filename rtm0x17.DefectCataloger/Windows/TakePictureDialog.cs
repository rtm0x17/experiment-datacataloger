using OpenCvSharp.Extensions;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace rtm0x17.DefectCataloger.Windows;

public partial class TakePictureDialog : Form
{
    public byte[] Image { get; private set; } = Array.Empty<byte>();
    private VideoCapture _videoCapture;
    private Mat _matFrame;
    private Bitmap _image;
    private Thread _cameraThread;
    private bool _cameraLoaded = false;

    public TakePictureDialog() => InitializeComponent();

    private void CaptureCamera()
    {
        _cameraThread = new Thread(new ThreadStart(CaptureCameraCallback));
        _cameraThread.Start();
    }

    private void CaptureCameraCallback()
    {
        _matFrame = new Mat();
        _videoCapture = new VideoCapture(0);
        _videoCapture.Open(0);

        if (_videoCapture.IsOpened())
            while (_cameraLoaded)
                try
                {
                    _videoCapture.Read(_matFrame);
                    _image = BitmapConverter.ToBitmap(_matFrame);

                    //if (PictureBoxArea.Image != null)
                    //    PictureBoxArea.Image.Dispose();

                    PictureBoxArea.Image = _image;

                    Thread.Sleep(1000);
                }
                catch
                {
                    Thread.Sleep(1000);
                }
    }

    private void TakePictureDialog_FormClosing(object sender, FormClosingEventArgs e)
    {

    }

    private void TakePictureDialog_Load(object sender, EventArgs e)
    {
        CaptureCamera();
        _cameraLoaded = true;
    }

    private void ButtonTakePicture_Click(object sender, EventArgs e)
    {
        if (!_cameraLoaded || PictureBoxArea.Image is null)
        {
            Image = Array.Empty<byte>();
            return;
        }

        var snapshot = new Bitmap(PictureBoxArea.Image);
        var memoryStream = new MemoryStream();
        snapshot.Save(memoryStream, ImageFormat.Jpeg);
        Image = memoryStream.ToArray();

        Hide();
    }
}
