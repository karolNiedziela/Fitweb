using Backend.Infrastructure.CommandHandler.Commands;
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

        public FileController(ICommandDispatcher commandDispatcher, IFileService fileService, ILoggerManager logger) 
            : base(commandDispatcher)
        {
            _fileService = fileService;
            _logger = logger;
        }

        
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (file is null)
            {
                return NotFound();
            }

            var filePath = _fileService.CreateFilePath(file);

            await _fileService.CreateFile(file, filePath);

            _logger.LogInfo($"File with filename: {file.FileName} created in {filePath}.");

            return Ok();
        }
    }
}
