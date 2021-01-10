using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
