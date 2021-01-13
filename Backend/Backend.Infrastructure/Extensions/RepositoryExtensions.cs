using Backend.Core.Entities;
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
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with id: {userId} was not found.");
            }

            return user;
        }

        public static async Task<Athlete> GetOrFailAsync(this IAthleteRepository repository, int userId)
        {
            var athlete = await repository.GetAsync(userId);
            if (athlete == null)
            {
                throw new ServiceException(ErrorCodes.AthleteNotFound, $"Athlete with user id: {userId} was not found.");
            }

            return athlete;
        }

        public static async Task CheckIfAthleteAsync(this IAthleteRepository repository, User user, string roleName)
        {
            var athlete = await repository.GetAsync(user.Id);
            if (athlete is null && roleName != "Admin")
            {
                athlete = new Athlete(user);
                await repository.AddAsync(athlete);
            }
        }

        public static async Task<string> CheckIfAdminExists(this IUserRepository repository, string roleName)
        {
            if (await repository.AnyAsync(u => u.UserRoles.Any(ur => ur.Role.Name.ToString() == "Admin")))
            {
                return roleName = "Admin";
            }

            return roleName;
        }
    }
}
