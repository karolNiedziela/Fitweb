using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Utilities.Csv;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class ExerciseInitializer : IExerciseInitializer
    {
        private readonly ICsvLoader<Exercise, ExerciseMap> _loader;
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseInitializer(ICsvLoader<Exercise, ExerciseMap> loader, IExerciseRepository exerciseRepository)
        {
            _loader = loader;
            _exerciseRepository = exerciseRepository;
        }

        public async Task LoadFromCsv()
        {
            var stringPath = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            stringPath += @"/Files/exercises.csv";

            var exercises = _loader.LoadCsvAsync(stringPath);

            await _exerciseRepository.AddRangeAsync(exercises);
        }
    }
}
