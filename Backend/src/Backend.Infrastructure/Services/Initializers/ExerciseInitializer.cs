using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Services.Logger;
using Backend.Infrastructure.Utilities.Csv;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class ExerciseInitializer : DataInitializer
    {
        private readonly ICsvLoader<Exercise, ExerciseMap> _loader;
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseInitializer(FitwebContext context, ILoggerManager logger,
            ICsvLoader<Exercise, ExerciseMap> loader,  IExerciseRepository exerciseRepository) : base(context, logger)
        {
            _loader = loader;
            _exerciseRepository = exerciseRepository;
        }

        public override async Task SeedAsync()
        {
            if (await _context.Exercises.AnyAsync())
            {
                return;
            }

            var stringPath = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            stringPath += @"/Files/exercises.csv";

            var exercises = _loader.LoadCsvAsync(stringPath);

            await _exerciseRepository.AddRangeAsync(exercises);

            _logger.LogInfo("Exercises added from exercises.csv");
        }
    }
}
