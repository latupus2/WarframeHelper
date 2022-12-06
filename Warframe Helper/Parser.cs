using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebsiteParser;
using WebsiteParser.Attributes.StartAttributes;

using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using static Warframe_Helper.JsonObject;
using System.Threading;
using static Warframe_Helper._JsonAllItems;
using static Warframe_Helper.MainWindow;

namespace Warframe_Helper
{
    public class Parser 
    {
        static string[] lines = File.ReadAllLines("Lists/itemListUrl.txt");
        static string[] AllPrices = new string[lines.Length];

        public void StartParse()
        {
            Console.WriteLine("\n" + DateTime.Now.ToString("HH:mm:ss") + " || Началось обновление цен");
            short counter = 0;
            foreach (string line in lines)
            {
                getItemOrders(line, counter);
                counter++;
            }
            File.WriteAllLines("Lists/Prices.txt", AllPrices);
            Console.WriteLine("\n" + DateTime.Now.ToString("HH:mm:ss") + " || Все цены обновлены");
            Params.PricesIsUpdateNow = false;
        }
        private void getItemOrders(string name, short counter)
        {
            string url = "https://api.warframe.market/v1/items/" + name +"/orders";
            var response = CallUrl(url).Result;
            Thread.Sleep(10);

            Rootobject jsObj = JsonConvert.DeserializeObject<Rootobject>(response);
            int CountOrders = jsObj.payload.orders.Length;
            List<int> Prices = new List<int> { };

            for (int i = 0; i < CountOrders; ++i)
            {
                if(jsObj.payload.orders[i].order_type == "sell" && jsObj.payload.orders[i].region == "en" && jsObj.payload.orders[i].user.status == "ingame")
                {
                    Prices.Add(jsObj.payload.orders[i].platinum);
                }
            }

            int[] _Prices = Prices.ToArray<int>();
            Array.Sort(_Prices);

            int num;
            if(_Prices.Length-1 < 5)
            {
                num = _Prices.Length-1;
            }else
            {
                num = 5;
            }

            int price = 0;
            for(short i = 0; i <= num; ++i)
            {
                price += _Prices[i];
            }
            price /= 5;
            AllPrices[counter] = price.ToString();
            Console.WriteLine(name +" -> "+ price +" платины");
        }
        public void getAllItemsPrices()
        {
            string url = "https://warframe.market/tools/ducats";
            var response = CallUrl(url).Result;
            //Console.WriteLine(response);
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(response);
            var document = html.DocumentNode;
            Console.WriteLine(document.QuerySelectorAll("script"));
        }
        public void getAllItemsInfo()
        {
            Console.WriteLine("\n"+DateTime.Now.ToString("HH: mm:ss")+" || Обновляем id предметов");
            string url = "https://api.warframe.market/v1/items";
            var response = CallUrl(url).Result;
            JsonAllItems jsItems = JsonConvert.DeserializeObject<JsonAllItems>(response);
            int CountItems = jsItems.payload.items.Length;

            string[] nameENG = new string[lines.Length];
            string[] id = new string[lines.Length];
            for (int i = 0; i < lines.Length; ++i)
            {
                for(int j = 0; j < CountItems; ++j)
                {
                    if(jsItems.payload.items[j].url_name == lines[i])
                    {
                        itemsList[i].nameENG = jsItems.payload.items[j].item_name;
                        nameENG[i] = jsItems.payload.items[j].item_name;
                        itemsList[i].id = jsItems.payload.items[j].id;
                        id[i] = jsItems.payload.items[j].id;
                    }
                }
            }
            File.WriteAllLines("Lists/itemListEng.txt", nameENG);
            File.WriteAllLines("Lists/itemListId.txt", id);
            Console.WriteLine(DateTime.Now.ToString("HH: mm:ss")+" || Обновление завершено");

        }
        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;

        }
    }

    

}
