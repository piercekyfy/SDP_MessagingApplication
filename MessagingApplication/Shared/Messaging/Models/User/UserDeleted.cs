using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging.Models.User
{
    public class UserDeleted : MessagingModel
    {
        public string UniqueName { get; set; }
        public DateTimeOffset DeletedAt { get; set; }

        public UserDeleted(string uniqueName, DateTimeOffset deletedAt)
        {
            UniqueName = uniqueName;
            DeletedAt = deletedAt;
        }
    }
}
