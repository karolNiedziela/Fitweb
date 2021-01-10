using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var athlete = await _athleteRepository.GetProductsAsync(userId);

            return _mapper.Map<AthleteDto>(athlete);
        }
         
        public async Task<AthleteDto> GetProductAsync(int userId, int productId)
        {
            var athlete = await _athleteRepository.GetProductAsync(userId, productId);

            return _mapper.Map<AthleteDto>(athlete);
        }

        public async Task<AthleteDto> GetExercisesAsync(int userId)
        {
            var athlete = await _athleteRepository.GetExercisesAsync(userId);

            return _mapper.Map<AthleteDto>(athlete);
        }

        public async Task<AthleteDto> GetExerciseAsync(int userId, int exerciseId)
        {
            var athlete = await _athleteRepository.GetExerciseAsync(userId, exerciseId);

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
