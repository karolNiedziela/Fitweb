using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<User> GetOrFailAsync(this IUserRepository repository, int userId)
        {
            var user = await repository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            return user;
        }

        public static async Task<Athlete> GetOrFailAsync(this IAthleteRepository repository, int userId)
        {
            var athlete = await repository.GetAsync(userId);
            if (athlete == null)
            {
                throw new AthleteNotFoundException(userId);
            }

            return athlete;
        }

        public static async Task<Exercise> GetOrFailAsync(this IExerciseRepository repository, int exerciseId)
        {
            var exercise = await repository.GetAsync(exerciseId);
            if (exercise == null)
            {
                throw new ExerciseNotFoundException(exerciseId);
            }

            return exercise;
        }

        public static async Task<Exercise> CheckIfExistsAsync(this IExerciseRepository repository, string name)
        {
            var exercise = await repository.GetAsync(name);
            if (exercise is not null)
            {
                throw new NameInUseException(nameof(Exercise), name);
            }

            return exercise;
        }

        public static async Task<Product> GetOrFailAsync(this IProductRepository repository, int productId)
        {
            var product = await repository.GetAsync(productId);
            if (product is null)
            {
                throw new ProductNotFoundException(productId);
            }

            return product;
        }

        public static async Task<Product> CheckIfExistsAsync(this IProductRepository repository, string name)
        {
            var product = await repository.GetAsync(name);
            if (product is not null)
            {
                throw new NameInUseException(nameof(Product), name);
            }

            return product;
        }

        public static async Task CheckIfAthleteAsync(this IAthleteRepository repository, User user, string roleName)
        {
            var athlete = await repository.GetAsync(user.Id);
            if (athlete is null && roleName != RoleId.Admin.ToString())
            {
                athlete = new Athlete(user);
                await repository.AddAsync(athlete);
            }
        }

        public static async Task<string> CheckIfAdminExists(this IUserRepository repository)
        {
            if (await repository.AnyAsync(u => u.UserRoles.Any(ur => ur.Role.Name == RoleId.Admin.ToString())))
            {
                return RoleId.User.ToString();
            }

            return RoleId.Admin.ToString();
        }
    }
}
