using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.Services.File;
using Backend.Infrastructure.Services.Logger;
using Backend.Infrastructure.Utilities.Csv;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ApiControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ILoggerManager _logger;

        public FileController(IDispatcher dispatcher, IFileService fileService, ILoggerManager logger) 
            : base(dispatcher)
        {
            _fileService = fileService;
            _logger = logger;
        }

        
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var filePath = _fileService.CreateFilePath(file);

            await _fileService.CreateFile(file, filePath);

            _logger.LogInfo($"File with filename: {file.FileName} created in {filePath}.");

            return Ok();
        }
    }
}
