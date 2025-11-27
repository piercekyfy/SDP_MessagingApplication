using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public class DBConfiguration
    {
        public string ConnectionString { get; set; } = default!;
        public string? Database { get; set; }
    }
}
