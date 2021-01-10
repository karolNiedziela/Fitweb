using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.Services.File;
using Backend.Infrastructure.Utilities.Csv;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public FileController(ICommandDispatcher commandDispatcher, IFileService fileService) 
            : base(commandDispatcher)
        {
            _fileService = fileService;
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

            return Ok();
        }
    }
}
