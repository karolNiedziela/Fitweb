using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler.Queries;
using Backend.Infrastructure.CommandQueryHandler.Queries.Exercises;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ApiControllerBase
    {
        private readonly ILoggerManager _logger;

        public ExercisesController(IDispatcher dispatcher, ILoggerManager logger) 
            : base(dispatcher)
        {
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExerciseDto>> Get(int id)
        {
            var exercise = await QueryAsync(new GetExercise(id));
            if (exercise is null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExerciseDto>> Get(string name)
        {
            var exercise = await QueryAsync(new GetExerciseByName(name));
            if (exercise is null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAll([FromQuery]GetExercises query)
        {
            var exercises = await QueryAsync(query);

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> Search([FromQuery]SearchExercises query)
        {
            var results = await QueryAsync(query);

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
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Post([FromBody]AddExercise command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Exercise with name: {command.Name} added.");

            return CreatedAtAction(nameof(Get), new { id = command.Id }, command);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete([FromBody]DeleteExercise command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Exercise with id: {command.ExerciseId} removed.");

            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Put([FromBody]UpdateExercise command)
        {
            await DispatchAsync(command);

            _logger.LogInfo($"Exercise with id: {command.ExerciseId} updated.");

            return Ok();
        }
    }
}
