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
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;

        public ExerciseService(IExerciseRepository exerciseRepository,
            IMapper mapper)
        {
            _exerciseRepository = exerciseRepository;
            _mapper = mapper;
        }

        public async Task<ExerciseDto> GetAsync(int id)
        {
            var exercise = await _exerciseRepository.GetAsync(id);

            return _mapper.Map<Exercise, ExerciseDto>(exercise);
        }

        public async Task<ExerciseDto> GetAsync(string name)
        {
            var exercise = await _exerciseRepository.GetAsync(name);

            return _mapper.Map<Exercise, ExerciseDto>(exercise);
        }

        public async Task<IEnumerable<ExerciseDto>> GetAllAsync()
        {
            var exercises = await _exerciseRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Exercise>, IEnumerable<ExerciseDto>>(exercises);
        }

        public async Task AddAsync(string partOfBody, string name)
        {
            var exercise = await _exerciseRepository.GetAsync(name);
            if (exercise != null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectAlreadyAdded,
                    $"Exercise with name: '{name}' already exists.");
            }

            exercise = new Exercise(partOfBody, name);
            await _exerciseRepository.AddAsync(exercise);
        }
        public async Task DeleteAsync(int id)
        {
            var exercise = await _exerciseRepository.GetAsync(id);
            if (exercise == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"Exercise with id: '{id}' was not found.");
            }

            await _exerciseRepository.DeleteAsync(exercise);
        }

        public async Task DeleteAsync(string name)
        {
            var exercise = await _exerciseRepository.GetAsync(name);
            if (exercise == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"Exercise with name: '{name}' was not found.");
            }

            await _exerciseRepository.DeleteAsync(exercise);
        }

        public async Task UpdateAsync(string partOfBody, string name)
        {
            var exercise = await _exerciseRepository.GetAsync(name);
            if (exercise == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"Exercise with name: '{name}' was not found.");
            }

            await _exerciseRepository.UpdateAsync(exercise);
        }

        
    }
}
