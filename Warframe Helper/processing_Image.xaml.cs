using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenCvSharp;
using System.IO;

namespace Warframe_Helper
{
    public partial class processing_Image : System.Windows.Window
    {
        static bool Ready = false;

        public Mat imageMat = new Mat();
        public processing_Image()
        {
            Ready = false;
            InitializeComponent();
            SliderThreshold.Value = Params.Threshold;
            
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imageMat = new Mat(op.FileName);
                ScreenImage.Source = new BitmapImage(new Uri(op.FileName));
                Ready = true;
            }

        }

        private void SliderThreshold_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Ready)
            {
                Params.SetThreshold = (int)SliderThreshold.Value;
                ThresholdValue.Content = (int)SliderThreshold.Value;
                Processing();
            }
        }

        private void Processing()
        {
            Mat image = new Mat();
            image = imageMat;
            image = image.Threshold(Params.Threshold, 255, Params.ThresholdType);
            Bitmap BitImage = new Bitmap(image.ToMemoryStream());
            ImageSourceConverter c = new ImageSourceConverter();
            ScreenImage.Source = Convert(BitImage);
        }
        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            Params.PrImage = false;
        }
    }
}
