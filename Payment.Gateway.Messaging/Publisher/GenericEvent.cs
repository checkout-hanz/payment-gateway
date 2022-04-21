using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Gateway.Messaging.Publisher
{
    internal class GenericEvent : IEvent
    {
        public string EventName { get; set; }
    }
}
