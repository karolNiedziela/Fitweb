using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler
{
    public interface IDispatcher
    {
        Task DispatchAsync<T>(T command) where T : ICommand;

        Task<T> QueryAsync<T>(IQuery<T> query);
    }
}
