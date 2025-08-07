using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.EventBus
{
    public static class EventTypes
    {
        public const string BasketAdded = "BasketAdded";
        public const string BasketRemoved = "BasketRemoved";
        public const string OrderPlaced = "OrderPlaced";
        public const string OrderCancelled = "OrderCancelled";
        public const string PriceChanged = "PriceChanged";
    }
}
