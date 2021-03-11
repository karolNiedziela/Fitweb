using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IGlobalSeeder
    {
        Task SeedAsync();
    }
}