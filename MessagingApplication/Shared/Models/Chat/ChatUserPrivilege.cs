using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Chat
{
    public enum ChatUserPrivilege
    {
        IsAdmin = 1,
        CanSend = 2,
        CanForward = 3
    }
}
