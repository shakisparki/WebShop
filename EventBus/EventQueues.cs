using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.EventBus
{
    public static class EventQueues
    {
        public const string BasketQueue = "basketQueue";
        public const string CatalogQueue = "catalogQueue";
        public const string OrderQueue = "orderQueue";
    }
}
