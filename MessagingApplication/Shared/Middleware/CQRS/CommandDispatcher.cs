using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Middleware.CQRS
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken ct = default) where TCommand : ICommand<TResult>
        {
            var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>(); // Ugly dynamic is unavoidable here.
            if (handler == null)
                throw new InvalidOperationException($"No handler exists for command {typeof(TCommand).Name}.");

            return await handler.Execute(command);
        }
    }
}
