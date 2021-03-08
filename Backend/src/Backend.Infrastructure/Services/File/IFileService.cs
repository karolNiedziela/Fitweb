using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.File
{
    public interface IFileService
    { 
        string CreateFilePath(IFormFile file);

        Task CreateFile(IFormFile file, string filePath);
    }
}
