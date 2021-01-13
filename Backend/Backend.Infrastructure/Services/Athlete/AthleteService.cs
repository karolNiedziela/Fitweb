using AutoMapper;
using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class AthleteService : IAthleteService
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AthleteService(IAthleteRepository athleteRepository, IUserRepository userRepository, IMapper mapper)
        {
            _athleteRepository = athleteRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<AthleteDto> GetAsync(int userId)
        {
            var athlete = await _athleteRepository.GetAsync(userId);

            return _mapper.Map<AthleteDto>(athlete);
        }

        public async Task<IEnumerable<AthleteDto>> GetAllAsync()
        {
            var athletes = await _athleteRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<AthleteDto>>(athletes);
        }

        public async Task<AthleteDto> GetProductsAsync(int userId)
        {
            var athlete = await _athleteRepository.FindByCondition(a => a.UserId == userId)
                                                  .Include(a => a.AthleteProducts)
                                                    .ThenInclude(ap => ap.Product)
                                                        .ThenInclude(p => p.CategoryOfProduct)
                                                  .SingleOrDefaultAsync();

            return _mapper.Map<AthleteDto>(athlete);
        }
         
        public async Task<AthleteDto> GetProductAsync(int userId, int productId)
        {
            var athlete = await _athleteRepository.FindByCondition(a => a.UserId == userId)
                                                  .Include(a => a.AthleteProducts
                                                    .Where(ap => ap.ProductId == productId))
                                                    .ThenInclude(ap => ap.Product)
                                                        .ThenInclude(p => p.CategoryOfProduct)
                                                  .SingleOrDefaultAsync();

            return _mapper.Map<AthleteDto>(athlete);
        }

        public async Task<AthleteDto> GetExercisesAsync(int userId)
        {
            var athlete = await _athleteRepository.FindByCondition(a => a.UserId == userId)
                                                  .Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Day)
                                                  .Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Exercise)
                                                        .ThenInclude(e => e.PartOfBody)
                                                  .SingleOrDefaultAsync();

            return _mapper.Map<AthleteDto>(athlete);
        }

        public async Task<AthleteDto> GetExerciseAsync(int userId, int exerciseId)
        {
            var athlete = await _athleteRepository.FindByCondition(a => a.UserId == userId)
                                                  .Include(a => a.AthleteExercises
                                                    .Where(ae => ae.ExerciseId == exerciseId))
                                                    .ThenInclude(ae => ae.Day)
                                                  .Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Exercise)
                                                        .ThenInclude(e => e.PartOfBody)
                                                  .SingleOrDefaultAsync();

            return _mapper.Map<AthleteDto>(athlete);
        }

        public async Task CreateAsync(int userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            var athlete = await _athleteRepository.GetAsync(userId);
            if (athlete != null)
            {
                throw new Exception($"Athlete with user id: {userId} already exists.");
            }

            athlete = new Athlete(user);
            await _athleteRepository.AddAsync(athlete);
        }

        public async Task DeleteAsync(int userId)
        {
            var athlete = await _athleteRepository.GetOrFailAsync(userId);

            await _athleteRepository.DeleteAsync(athlete);
        }

    }
}
