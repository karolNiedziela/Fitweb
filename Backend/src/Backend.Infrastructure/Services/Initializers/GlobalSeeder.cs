using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.Initializers
{
    public class GlobalSeeder : IGlobalSeeder
    {
        private readonly IEnumerable<IDataInitializer> _datas;

        public GlobalSeeder(IEnumerable<IDataInitializer> datas)
        {
            _datas = datas;
        }

        public async Task SeedAsync()
        {
            foreach (var data in _datas)
            {
                await data.SeedAsync();
            }
        }
    }
}
