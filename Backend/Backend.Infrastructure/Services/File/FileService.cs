using Backend.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.File
{
    public class FileService : IFileService
    {
        public string CreateFilePath(IFormFile file)
        {
            if (file is null)
            {
                throw new ServiceException(ErrorCodes.InvalidValue, $"No file provided.");
            }

            var path = Directory.GetParent(Directory.GetCurrentDirectory());
            string filePath = $"{path}/files/{file.FileName}";

            return filePath;
        }

        public async Task CreateFile(IFormFile file, string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                throw new ServiceException(ErrorCodes.ObjectAlreadyAdded, $"File with {file.FileName} already saved.");
            }

            using var stream = System.IO.File.Create(filePath);
            await file.CopyToAsync(stream);
        }
    }
}
