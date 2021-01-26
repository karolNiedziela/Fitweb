using AutoMapper;
using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Core.Helpers;
using Backend.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Repositories;

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

        public async Task<PagedList<ExerciseDto>> GetAllAsync(PaginationQuery paginationQuery)
        {
            var exercises = await _exerciseRepository.GetAllAsync(paginationQuery);

            var exercisesDto = _mapper.Map<IEnumerable<Exercise>, IEnumerable<ExerciseDto>>(exercises).ToList();

            return new PagedList<ExerciseDto>(exercisesDto, exercises.TotalCount, exercises.CurrentPage, exercises.PageSize);
        }

        public async Task<PagedList<ExerciseDto>> SearchAsync(PaginationQuery paginationQuery, string name, string partOfBody = null)
        {
            if (partOfBody is not null && PartOfBody.GetPart(partOfBody) is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"{partOfBody} does not exist.");
            }
            var exercises = await _exerciseRepository.SearchAsync(paginationQuery, name, partOfBody);

            var exercisesDto = _mapper.Map<IEnumerable<Exercise>, IEnumerable<ExerciseDto>>(exercises).ToList();

            return new PagedList<ExerciseDto>(exercisesDto, exercises.TotalCount, exercises.CurrentPage, exercises.PageSize);
        }

        public async Task<int> AddAsync(string name, string partOfBody)
        {
            var exercise = await _exerciseRepository.GetAsync(name);
            if (exercise is not null)
            {
                throw new ServiceException(ErrorCodes.ObjectAlreadyAdded,
                    $"Exercise with name: '{name}' already exists.");
            }

            exercise = new Exercise(name, PartOfBody.GetPart(partOfBody));

            await _exerciseRepository.AddAsync(exercise);

            return exercise.Id;
        }
        public async Task DeleteAsync(int id)
        {
            var exercise = await _exerciseRepository.GetAsync(id);
            if (exercise is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound,
                    $"Exercise with id: '{id}' was not found.");
            }

            await _exerciseRepository.DeleteAsync(exercise);
        }

        public async Task UpdateAsync(int id, string name, string partOfBody)
        {
            var exercise = await _exerciseRepository.GetAsync(id);
            if (exercise is null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"Exercise with name: '{name}' was not found.");
            }

            if (await _exerciseRepository.AnyAsync(p => p.Name == name))
            {
                if (exercise.Name != name)
                {
                    throw new ServiceException(ErrorCodes.ObjectAlreadyAdded, $"Exercise with '{name}' already exists.");
                }
            }

            exercise.SetName(name);
            exercise.PartOfBody.Id = PartOfBody.GetPart(partOfBody).Id;

            await _exerciseRepository.UpdateAsync(exercise);
        }



    }
}
