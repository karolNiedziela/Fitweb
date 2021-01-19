using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler.Queries;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ApiControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly ILoggerManager _logger;

        public ExercisesController(IDispatcher dispatcher, IExerciseService exerciseService,
            ILoggerManager logger) 
            : base(dispatcher)
        {
            _exerciseService = exerciseService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var exercise = await _exerciseService.GetAsync(id);
            if (exercise is null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var exercise = await _exerciseService.GetAsync(name);
            if (exercise is null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]GetExercises query)
        {
            var exercises = await _dispatcher.QueryAsync(query);

            var metadata = new
            {
                exercises.TotalCount,
                exercises.PageSize,
                exercises.CurrentPage,
                exercises.TotalPages,
                exercises.HasNext,
                exercises.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(exercises);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]SearchExercises query)
        {
            var results = await _dispatcher.QueryAsync(query);

            var metadata = new
            {
                results.TotalCount,
                results.PageSize,
                results.CurrentPage,
                results.TotalPages,
                results.HasNext,
                results.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(results);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody]AddExercise command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Exercise with name: {command.Name} added.");

            return CreatedAtAction(nameof(Get), new { id = command.Id }, command);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromBody]DeleteExercise command)
        {
            await _exerciseService.DeleteAsync(command.ExerciseId);

            _logger.LogInfo($"Exercise with id: {command.ExerciseId} removed.");

            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody]UpdateExercise command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Exercise with id: {command.ExerciseId} updated.");

            return CreatedAtAction(nameof(Get), new { id = command.ExerciseId }, command);
        }
    }
}
