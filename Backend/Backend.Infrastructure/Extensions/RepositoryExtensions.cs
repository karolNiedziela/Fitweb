using Backend.Core.Entities;
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
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with id: {userId} was not found.");
            }

            return user;
        }

        public static async Task<Athlete> GetOrFailAsync(this IAthleteRepository repository, int id)
        {
            var athlete = await repository.GetAsync(id);
            if (athlete == null)
            {
                throw new ServiceException(ErrorCodes.AthleteNotFound, $"Athlete with id: {id} was not found.");
            }

            return athlete;
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
            if (await repository.AnyAsync(u => u.UserRoles.Any(ur => ur.Role.Name == Role.GetRole(RoleId.Admin.ToString()).Name)))
            {
                return RoleId.User.ToString();
            }

            return RoleId.Admin.ToString();
        }
    }
}
