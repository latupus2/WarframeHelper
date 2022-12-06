using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Helper
{
    internal class _JsonAllItems
    {
        public class JsonAllItems
        {
            public Payload payload { get; set; }
        }

        public class Payload
        {
            public Item[] items { get; set; }
        }

        public class Item
        {
            public string id { get; set; }
            public string url_name { get; set; }
            public string thumb { get; set; }
            public string item_name { get; set; }
        }

    }
}
