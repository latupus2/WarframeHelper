using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Helper
{
    public class JsonObject
    {
        public class Rootobject
        {
            public Payload payload { get; set; }
        }

        public class Payload
        {
            public Order[] orders { get; set; }
        }

        public class Order
        {
            public string order_type { get; set; }
            public int platinum { get; set; }
            public int quantity { get; set; }
            public User user { get; set; }
            public string platform { get; set; }
            public string region { get; set; }
            public DateTime creation_date { get; set; }
            public DateTime last_update { get; set; }
            public bool visible { get; set; }
            public string id { get; set; }
        }

        public class User
        {
            public int reputation { get; set; }
            public string region { get; set; }
            public DateTime last_seen { get; set; }
            public string ingame_name { get; set; }
            public string avatar { get; set; }
            public string id { get; set; }
            public string status { get; set; }
        }

    }
}
