using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Gateway.Messaging.Publisher
{
    public interface IEvent
    {
        public string EventName { get; set; }
    }
}
