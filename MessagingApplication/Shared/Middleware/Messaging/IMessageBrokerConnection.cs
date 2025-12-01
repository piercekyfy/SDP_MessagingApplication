using RabbitMQ.Client;
using Shared.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Shared.Middleware.Messaging
{
    

    public interface IMessageBrokerConnection
    {
        public Task<IChannel> GetChannel();
    }
}
