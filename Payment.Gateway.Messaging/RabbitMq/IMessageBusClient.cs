using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Gateway.Messaging.RabbitMq
{
    public interface IMessageBusClient
    {
        void Publish(string content);
    }
}
