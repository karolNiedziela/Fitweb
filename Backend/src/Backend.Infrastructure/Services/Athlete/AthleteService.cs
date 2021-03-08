using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILoggerManager _logger;

        public AthleteService(IAthleteRepository athleteRepository, IUserRepository userRepository, IMapper mapper,
            ILoggerManager logger)
        {
            _athleteRepository = athleteRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AthleteDto> GetAsync(int id)
        {
            var athlete = await _athleteRepository.GetAsync(id);

            return _mapper.Map<AthleteDto>(athlete);
        }

        public async Task<IEnumerable<AthleteDto>> GetAllAsync()
        {
            var athletes = await _athleteRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<AthleteDto>>(athletes);
        }

        public async Task<AthleteDto> GetProductsAsync(int id, DateTime? date)
        {

            if (date is null)
            {
                date = DateTime.Today;
            }

            var athlete = await _athleteRepository.FindByCondition(condition: a => a.Id == id,
                include: source => source.Include(a => a.AthleteProducts.Where(ap => ap.DateCreated.Date == date))
                                                    .ThenInclude(ap => ap.Product)
                                                        .ThenInclude(p => p.CategoryOfProduct));
            

            return _mapper.Map<AthleteDto>(athlete);
        }
         
        public async Task<AthleteDto> GetProductAsync(int id, int productId)
        {
            var athlete = await _athleteRepository.FindByCondition(condition: a => a.Id == id,
                include: source => source.Include(a => a.AthleteProducts
                                                   .Where(ap => ap.ProductId == productId))
                                                    .ThenInclude(ap => ap.Product)
                                                        .ThenInclude(p => p.CategoryOfProduct));
                                                
            return _mapper.Map<AthleteDto>(athlete);
        }

        public async Task<AthleteDto> GetExercisesAsync(int id, string dayName)
        {
            var athlete = await _athleteRepository.FindByCondition(condition: a => a.Id == id,
                include: source => source.Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Day)
                                                  .Include(a => a.AthleteExercises.Where(ae => ae.DayId == Day.GetDay(dayName).Id))
                                                    .ThenInclude(ae => ae.Exercise)
                                                        .ThenInclude(e => e.PartOfBody));

            return _mapper.Map<AthleteDto>(athlete);
        }

        public async Task<AthleteDto> GetExerciseAsync(int id, int exerciseId)
        {
            var athlete = await _athleteRepository.FindByCondition(condition: a => a.Id == id,
                include: source => source.Include(a => a.AthleteExercises
                                                    .Where(ae => ae.ExerciseId == exerciseId))
                                                    .ThenInclude(ae => ae.Day)
                                                  .Include(a => a.AthleteExercises)
                                                    .ThenInclude(ae => ae.Exercise)
                                                        .ThenInclude(e => e.PartOfBody));

            return _mapper.Map<AthleteDto>(athlete);
        }

        public async Task<int> CreateAsync(int userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            var athlete = await _athleteRepository.FindByCondition(a => a.UserId == userId);
            if (athlete != null)
            {
                throw new ServiceException(ErrorCodes.ObjectAlreadyAdded, $"Athlete with user id: {userId} already exists.");
            }

            athlete = new Athlete(user);
            await _athleteRepository.AddAsync(athlete);

            return athlete.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var athlete = await _athleteRepository.GetOrFailAsync(id);

            _logger.LogInfo($"Athlete with id: {id} was removed.");

            await _athleteRepository.DeleteAsync(athlete);
        }

    }
}
