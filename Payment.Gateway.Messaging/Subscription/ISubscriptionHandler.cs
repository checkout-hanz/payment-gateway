using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payment.Gateway.Messaging.Publisher;

namespace Payment.Gateway.Messaging.Subscription
{
    public interface ISubscriptionHandler<in T> where T : IMessage
    {
        public Task Handle(T message);
    }
}
