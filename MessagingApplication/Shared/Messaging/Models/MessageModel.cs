using Shared.Messaging.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Messaging.Models
{
    public abstract class MessageModel
    {
        public virtual Memory<byte> AsJsonBytes()
        {
            return Encoding.UTF8.GetBytes(JsonSerialize());
        }
        public virtual string JsonSerialize()
        {
            return JsonSerializer.Serialize(this);     
        }
    }
}
