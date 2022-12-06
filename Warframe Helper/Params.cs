using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Helper
{
    public class Params
    {
        public static string Text = "";
        public static bool PricesIsUpdateNow = false;
        public static short ItemsCount = 391;


        public static bool PrImage = false;
        public static int Threshold = 140;
        public static ThresholdTypes ThresholdType = ThresholdTypes.Binary;

        public static int SetThreshold
        {
            set
            {        
                if (value >= 0)
                {
                    Threshold = value;
                    ThresholdType = ThresholdTypes.Binary;
                }
                else
                {
                    Threshold = value * -1;
                    ThresholdType = ThresholdTypes.BinaryInv;
                }
            }
        }


    }
    public class CursorPosition
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }

    public class items
    {
        public string id { get; set; }
        public string nameRUS { get; set; }
        public string nameENG { get; set; }
        public string nameUrl { get; set; }
        public int platinum { get; set; }
        public int ducats { get; set; }

    }
}
