using Backend.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IRoleRepository : IRepository
    {
        Task<Role> GetAsync(string name);
    }
}
