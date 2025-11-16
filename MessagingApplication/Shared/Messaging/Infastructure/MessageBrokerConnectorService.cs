using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging.Infastructure
{
    public class MessageBrokerConnectorService : IHostedService
    {
        private readonly IMessageBrokerConnection connection;

        public MessageBrokerConnectorService(IMessageBrokerConnection connection)
        {
            this.connection = connection;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await connection.Connect();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
