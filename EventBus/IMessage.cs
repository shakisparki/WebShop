using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.EventBus
{
    public interface IMessage
    {
        public string Type { get; set; }
    }
}
