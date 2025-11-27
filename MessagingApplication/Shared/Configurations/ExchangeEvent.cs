using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public class ExchangeEvent
    {
        public string Name { get; set; }
        public string Route { get; set; }

        public ExchangeEvent(string name, string route)
        {
            Name = name;
            Route = route;
        }
    }
}
