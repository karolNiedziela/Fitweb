using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IExerciseService
    {
        Task<ExerciseDto> GetAsync(int id);

        Task<ExerciseDto> GetAsync(string name);

        Task<PagedList<ExerciseDto>> GetAllAsync(PaginationQuery paginationQuery);

        Task<PagedList<ExerciseDto>> SearchAsync(PaginationQuery paginationQuery, string name, string partOfBody = null);

        Task<int> AddAsync(string name, string partOfBody);

        Task DeleteAsync(int id);

        Task UpdateAsync(int id, string name, string partOfBody);
    }
}
