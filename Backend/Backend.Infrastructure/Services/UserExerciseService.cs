using AutoMapper;
using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class UserExerciseService : IUserExerciseService
    {
        private readonly IUserRepository _userRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUserExerciseRepository _userExerciseRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IMapper _mapper;

        public UserExerciseService(IUserRepository userRepository, IExerciseRepository exerciseRepository, IUserExerciseRepository userExerciseRepository,
            IDayRepository dayRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _exerciseRepository = exerciseRepository;
            _userExerciseRepository = userExerciseRepository;
            _dayRepository = dayRepository;
            _mapper = mapper;
        }

        public async Task<UserExercise> GetAsync(int userExerciseId)
        {
            var userExercise = await _userExerciseRepository.GetAsync(userExerciseId);

            return _mapper.Map<UserExercise>(userExercise);
        }

        public async Task<UserExercise> GetAsync(int userId, int exerciseId)
        {
            var userExercise = await _userExerciseRepository.GetAsync(userId, exerciseId);

            return _mapper.Map<UserExercise>(userExercise);
        }

        public async Task<IEnumerable<UserExercise>> GetAllExercisesForUserAsync(int userId)
        {
            var userExercises = await _userExerciseRepository.GetAllExercisesForUserAsync(userId);

            return _mapper.Map<IEnumerable<UserExercise>>(userExercises);
        }

        public async  Task<IEnumerable<UserExercise>> GetAllAsync()
        {
            var userExercises = await _userExerciseRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserExercise>>(userExercises);
        }

        public async Task AddAsync(int userId, int exerciseId, double weight, int numberOfSets,
            int numberOfReps, string dayName)
        {

            var day = await _dayRepository.GetAsync(dayName);
            var userExercise = await _userExerciseRepository.GetAsync(userId, exerciseId);
            if (userExercise != null && userExercise.Day == day)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"User with id: {userExercise.UserId} already has exercise with id: {userExercise.ExerciseId}");
            }

            var user = await _userRepository.GetAsync(userId);
            var exercise = await _exerciseRepository.GetAsync(exerciseId);

           
            user.AddExercise(user, exercise, weight, numberOfSets, numberOfReps, day);
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int userExerciseId)
        {
            var userExercise = await _userExerciseRepository.GetAsync(userExerciseId);
            if (userExercise == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"Exercise with id: {userExercise.ExerciseId} for user with id: {userExercise.UserId} was not found");
            }

            await _userExerciseRepository.DeleteAsync(userExercise);

        }

        public async Task UpdateAsync(int userExerciseId, int userId, int exerciseId, double weight, int numberOfSets, int numberOfReps, string dayName)
        {
            var userExercise = await _userExerciseRepository.GetAsync(userExerciseId);
            if (userExercise == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                        "Exercise does not exist.");
            }

            var day = await _dayRepository.GetAsync(dayName);

            userExercise.SetWeight(weight);
            userExercise.SetNumberOfSets(numberOfSets);
            userExercise.SetNumberOfReps(numberOfReps);
            userExercise.SetDay(day);

            await _userExerciseRepository.UpdateAsync(userExercise);
        }

       
    }
}
