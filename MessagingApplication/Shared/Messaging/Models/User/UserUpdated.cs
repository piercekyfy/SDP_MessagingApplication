using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging.Models.User
{
    public class UserUpdated : MessagingModel
    {
        public string UniqueName { get; set; }
        public string DisplayName { get; set; }

        public UserUpdated(string uniqueName, string displayName)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
        }
    }
}
