using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Shared.Messaging.Infastructure
{
    public interface IMessageBrokerConnection
    {
        public Task Connect();

        public Task DeclareExchange(string exchange, string type);

        public Task BasicPublish(string exchange, string routingKey, ReadOnlyMemory<byte> body, CancellationToken ct = default);
    }
}
