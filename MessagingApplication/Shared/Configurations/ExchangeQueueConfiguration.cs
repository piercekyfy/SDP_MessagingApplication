using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public abstract class ExchangeQueueConfiguration
    {
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }

        public ExchangeQueueConfiguration(string name, string exchange, string routingKey, Dictionary<string, string> events)
        {
            Name = name;
            Exchange = exchange;
            RoutingKey = routingKey;
        }
    }
}