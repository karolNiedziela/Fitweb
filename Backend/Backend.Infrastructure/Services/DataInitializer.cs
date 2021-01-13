using Backend.Infrastructure.EF;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IProductInitializer _productInitializer;
        private readonly FitwebContext _context;
        private readonly IExerciseInitializer _exerciseInitializer;

        public DataInitializer(IProductInitializer productInitializer, IExerciseInitializer exerciseInitializer, 
            FitwebContext context)
        {
            _context = context;
            _productInitializer = productInitializer;
            _exerciseInitializer = exerciseInitializer;
        }

        public async Task SeedAsync()
        {
            if (!_context.Products.Any())
            {
                await _productInitializer.LoadFromCsv();
            }

            if (!_context.Exercises.Any())
            {
                await _exerciseInitializer.LoadFromCsv();
            }
        }
    }
}
