using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Gateway.Messaging.RabbitMq
{
    public interface IRabbitMQConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class RabbitMQConfig : IRabbitMQConfig
    {
        public RabbitMQConfig(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public string Host { get; set; }
        public int Port { get; set; }
    }
}
