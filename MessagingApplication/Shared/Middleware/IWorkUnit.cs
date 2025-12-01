using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Middleware
{
    public interface IWorkUnit
    {
        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
