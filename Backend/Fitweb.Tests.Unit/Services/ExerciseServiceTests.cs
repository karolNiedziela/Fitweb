﻿using AutoMapper;
using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Core.Helpers;
using Backend.Infrastructure.Mappers;
using Backend.Core.Repositories;
using Backend.Infrastructure.Services;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Backend.Infrastructure.Repositories;

namespace Fitweb.Tests.Unit.Services
{
    public class ExerciseServiceTests : IClassFixture<FitwebSeedDataFixture>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;
        private readonly IExerciseService _sut;
        private readonly FitwebSeedDataFixture _fixture;

        public ExerciseServiceTests(FitwebSeedDataFixture fixture)
        {
            _fixture = fixture;
            _exerciseRepository = Substitute.For<IExerciseRepository>();
            _mapper = Substitute.For<IMapper>();
            _mapper = AutoMapperConfig.Initialize();
            _sut = new ExerciseService(_exerciseRepository, _mapper);
        }

        [Fact]
        public async Task GetAsyncById_ShouldReturnExerciseDto()
        {
            var exercise = new Exercise { Id = 1 };
            _exerciseRepository.GetAsync(1).Returns(exercise);

            var dto = await _sut.GetAsync(exercise.Id);

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(exercise.Id);
            dto.ShouldBeOfType(typeof(ExerciseDto));
            await _exerciseRepository.Received(1).GetAsync(exercise.Id);
        }

        [Fact]
        public async Task GetAsyncByName_ShouldReturnExerciseDto()
        {
            var exercise = new Exercise { Name = "chest push" };
            _exerciseRepository.GetAsync("chest push").Returns(exercise);

            var dto = await _sut.GetAsync(exercise.Name);

            dto.ShouldNotBeNull();
            dto.Name.ShouldBe(exercise.Name);
            dto.ShouldBeOfType(typeof(ExerciseDto));
            await _exerciseRepository.Received(1).GetAsync(exercise.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnIEnumerableExerciseDto()
        {
            var exercises = new List<Exercise>
            {
                new Exercise { Id = 1 },
                new Exercise { Id = 2 }
            };

            var pagedList = Substitute.For<PagedList<Exercise>>(exercises, 10, 10, 10);

            var paginationQuery = Substitute.For<PaginationQuery>();
            _exerciseRepository.GetAllAsync(paginationQuery).Returns(pagedList);

            var dto = await _sut.GetAllAsync(paginationQuery);

            dto.ShouldNotBeNull();
            dto.ShouldBeOfType(typeof(PagedList<ExerciseDto>));
            dto.Count().ShouldBe(2);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewExercise()
        {
            await _sut.AddAsync("chest push", "Chest");

            await _exerciseRepository.Received(1).AddAsync(Arg.Any<Exercise>());
        }

        [Fact] 
        public async Task AddAsync_ShouldThrowException_WhenExerciseNameIsNotUnique()
        {
            var exerciseRepository = Substitute.For<ExerciseRepository>(_fixture.FitwebContext);
            var exerciseService = Substitute.For<ExerciseService>(exerciseRepository, _mapper);

            var exception = await Record.ExceptionAsync(() =>  exerciseService.AddAsync("exercise1", "Chest"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), $"Exercise with name: 'exercise1' already exists.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteExercise_WhenExerciseExists()
        {
            var exerciseRepository = Substitute.For<ExerciseRepository>(_fixture.FitwebContext);
            var exerciseService = Substitute.For<ExerciseService>(exerciseRepository, _mapper);

            await exerciseService.DeleteAsync(2);

            var exercise = await exerciseRepository.GetAsync(2);
            exercise.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenExerciseDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() =>  _sut.DeleteAsync(1));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ServiceException));
            exception.ShouldBeOfType(typeof(ServiceException), "Exercise with id: 1 was not found.");

        }
    }
}
