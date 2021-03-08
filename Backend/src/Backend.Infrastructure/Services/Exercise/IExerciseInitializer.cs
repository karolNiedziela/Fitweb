using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IExerciseInitializer
    {
        Task LoadFromCsv();
    }
}
