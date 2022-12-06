using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Threading;
using GlobalHotKey;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Warframe_Helper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private static readonly string[] lines = File.ReadAllLines("itemListUrl.txt");
        public static items[] itemsList = new items[Params.ItemsCount];
   
        public MainWindow()
        {

            InitializeComponent();
            ProgramStart();

        }
        private void ProgramStart()
        {
            ConsoleManager.Show();
            CreateItemsList();
            CreateHotKeys();
            

            Console.WriteLine("Работаем.");
        }
        private void CreateItemsList()
        {
            string[] id = File.ReadAllLines("Lists/itemListId.txt");
            string[] nameEng = File.ReadAllLines("Lists/itemListEng.txt");
            string[] Url = File.ReadAllLines("Lists/itemListUrl.txt");
            string[] Platinum = File.ReadAllLines("Lists/Prices.txt");
            for (short i = 0; i < itemsList.Length; i++)
            {
                itemsList[i] = new items();
                //itemsList[i].nameRUS = 
                itemsList[i].id = id[i];
                itemsList[i].nameENG = nameEng[i];
                itemsList[i].nameUrl = Url[i];
                itemsList[i].platinum = Convert.ToInt32(Platinum[i]);
                //itemsList[i].ducats =
            }

        }
        private void CreateHotKeys()
        {
            HotKeyManager hotKeyManager = new HotKeyManager();
            var hotKeyMode1 = hotKeyManager.Register(Key.Q, ModifierKeys.Alt);
            var hotKeyMode2 = hotKeyManager.Register(Key.E, ModifierKeys.Alt);
            hotKeyManager.KeyPressed += HotKeyManagerPressed;
            
        }
        private void getDataPrices()
        {
            Params.PricesIsUpdateNow = true;
            Parser parser = new Parser();
            Thread Pars = new Thread(new ThreadStart(parser.StartParse));
            Pars.Start();
        }

        private void HotKeyManagerPressed(object sender, KeyPressedEventArgs e)
        {
            if (Params.PricesIsUpdateNow == false)
            {
                if (e.HotKey.Key == Key.Q)
                {
                    Console.WriteLine("\n" + DateTime.Now.ToString("HH:mm:ss") + " || Запуск одиночного режима");
                    StartMode1();
                }

                if (e.HotKey.Key == Key.E)
                {
                    Console.WriteLine("\n" + DateTime.Now.ToString("HH:mm:ss") + " || Запуск режима выделения");

                }
            }
            else
            {
                Console.WriteLine("\n========================================\n" + "\nЯ тут так-то занят\n" + "\n========================================\n");
            }
        }

        private void StartMode1()
        {
            

            Program program = new Program();

            Thread MainProgram = new Thread(new ThreadStart(program.Search));
            MainProgram.Start();

        }

        private void UpdatePricesButton_Click(object sender, RoutedEventArgs e)
        {
            if(Params.PricesIsUpdateNow == false)
            {
                getDataPrices();
            }
            else
            {
                Console.WriteLine("\n========================================\n" + "\nЦены уже и так обновляются\n"+ "\n========================================\n");
            }
            
        }

        private void ShowItemsButton_Click(object sender, RoutedEventArgs e)
        {
            for(short i=0; i < Params.ItemsCount; i++)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("id: " + itemsList[i].id);
                Console.WriteLine("nameRUS: "+itemsList[i].nameRUS);
                Console.WriteLine("nameENG: "+itemsList[i].nameENG);
                Console.WriteLine("Url: "+itemsList[i].nameUrl);
                Console.WriteLine("platinum: "+itemsList[i].platinum);
                Console.WriteLine("ducats: "+itemsList[i].ducats);
                Console.WriteLine("======================================");
            }

        }

        private void UpdateItemInfo_Click(object sender, RoutedEventArgs e)
        {
            Parser parser = new Parser();
            Thread Pars = new Thread(new ThreadStart(parser.getAllItemsInfo));
            Pars.Start();

        }

        private void UpdatePricesFast_Click(object sender, RoutedEventArgs e)
        {
            Parser parser = new Parser();
            Thread Pars = new Thread(new ThreadStart(parser.getAllItemsPrices));
            Pars.Start();

        }

        private void processingImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Params.PrImage == false)
            {
                Params.PrImage = true;
                processing_Image win = new processing_Image();
                win.Show();
            }
            else Console.WriteLine("Это окно уже открыто");
        }
    }

}
