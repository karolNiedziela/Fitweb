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
using Backend.Infrastructure.Extensions;

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

        public async Task<PageResultDto<ExerciseDto>> GetAllAsync(string name, string partOfBody, 
            PaginationQuery paginationQuery)
        {
            var exercises = await _exerciseRepository.GetAllAsync(name, partOfBody, paginationQuery);


            return _mapper.Map<PageResultDto<ExerciseDto>>(exercises);
        }

        public async Task<int> AddAsync(string name, string partOfBody)
        {
            var exercise = await _exerciseRepository.CheckIfExistsAsync(name);

            exercise = new Exercise(name, PartOfBody.GetPart(partOfBody));

            await _exerciseRepository.AddAsync(exercise);

            return exercise.Id;
        }
        public async Task DeleteAsync(int id)
        {
            var exercise = await _exerciseRepository.GetOrFailAsync(id);

            await _exerciseRepository.DeleteAsync(exercise);
        }

        public async Task UpdateAsync(int id, string name, string partOfBody)
        {
            var exercise = await _exerciseRepository.GetOrFailAsync(id);

            // If exercise has the same name, ensure that name will be searching for exercise with this name 
            if (await _exerciseRepository.AnyAsync(p => p.Name == name))
            {
                if (exercise.Name != name)
                {
                    throw new NameInUseException(nameof(Exercise), name);
                }
            }

            exercise.SetName(name);
            exercise.PartOfBody.Id = PartOfBody.GetPart(partOfBody).Id;

            await _exerciseRepository.UpdateAsync(exercise);
        }



    }
}
