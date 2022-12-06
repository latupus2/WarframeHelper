using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Tesseract;
using System.IO;
using OpenCvSharp;


namespace Warframe_Helper
{
    
    public class Program
    {
        public TextBlock MainText;
        public static byte StatusProgram = 1;
        static bool Error = false;

        public Program()
        {
            Console.WriteLine("Поток начал работу");
            Error = false;
        }
        public void Search()
        {
            CursorPosition cursorPosition = getCursorPosition();
            Console.WriteLine("X=" + cursorPosition.PositionX + " | " + "Y=" + cursorPosition.PositionY);
            var ScreenShot = getScreenShot(cursorPosition.PositionX, cursorPosition.PositionY);
            ScreenShot = processingScreenShot(ScreenShot);
            if (Error == false) getTextFromScreen(ScreenShot);

        }

        private static CursorPosition getCursorPosition()
        {
            return new CursorPosition {PositionX = Cursor.Position.X, PositionY = Cursor.Position.Y};
        }

        private static Bitmap getScreenShot(int CursorX, int CursorY)
        {
            var size = new System.Drawing.Size(150, 150);
            
            Bitmap printscreen = new Bitmap(150, 150);
            Graphics graphics = Graphics.FromImage(printscreen);
            graphics.CopyFromScreen(CursorX, CursorY- 150, 0, 0, size);
            try
            {
                printscreen.Save("ScreenShots/1.png", System.Drawing.Imaging.ImageFormat.Png);
            }catch (System.Runtime.InteropServices.ExternalException)
            {
                Console.WriteLine("На этой ноте я умер.");
                Error = true;
            }
            return printscreen;
        }
        private static Bitmap processingScreenShot(Bitmap ScreenShot)
        {

            Mat image = new Mat("ScreenShots/1.png");
            image = image.Threshold(140, 255, ThresholdTypes.Binary);
            Bitmap BitImage = new Bitmap(image.ToMemoryStream());

            try
            {
                BitImage.Save("ScreenShots/1Obrabotka.png");
            }
            catch { }

            return BitImage;
        }
        private static void getTextFromScreen(Bitmap image)
        {

            Console.WriteLine("Текст:");
            var ocrtext = string.Empty;
            //Bitmap image = new Bitmap("ScreenShots/1Obrabotka.png");


            using (var engine = new TesseractEngine(@"./tessdata-main", "rus", EngineMode.TesseractOnly))
            {
                using (var img = PixConverter.ToPix(image))
                {
                    using (var page = engine.Process(img, PageSegMode.SparseText))
                    {
                        ocrtext = page.GetText();

                    }
                }
            }
            Console.WriteLine(ocrtext);
            Console.WriteLine("ПРОЧИТАНО");
        }
    }
}
