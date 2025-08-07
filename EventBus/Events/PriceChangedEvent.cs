using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.EventBus.Events
{
    public class PriceChangedEvent : IMessage
    {
        public string Type { get; set; }
        public string ProductId { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
    }
}
