using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler
{
    public class Dispatcher : IDispatcher
    {
        private readonly IComponentContext _context;

        public Dispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task DispatchAsync<T>(T command) where T : ICommand
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command), $"Command '{typeof(T).Name}' cannot be null.");
            }

            var handler = _context.Resolve<ICommandHandler<T>>();
            await handler.HandleAsync(command);
        }

        public async Task<T> QueryAsync<T>(IQuery<T> query)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query), $"Query '{typeof(T).Name}' cannot be null.");
            }

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(T));
            dynamic handler = _context.Resolve(handlerType);
            return await handler.HandleAsync((dynamic)query);
        }
    }
}
