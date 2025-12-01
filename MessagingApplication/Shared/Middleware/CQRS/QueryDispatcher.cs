using Microsoft.Extensions.DependencyInjection;

namespace Shared.Middleware.CQRS
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken ct = default) where TQuery : IQuery<TResult>
        {
            var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>(); // Ugly dynamic is unavoidable here.
            if (handler == null)
                throw new InvalidOperationException($"No handler exists for query {typeof(TQuery).Name}.");

            return await handler.Execute(query);
        }
    }
}
