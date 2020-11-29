using Backend.Core.Domain;
using Backend.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IUserService _userService;
        private readonly IRoleRepository _roleRepository;
        private readonly IExerciseService _exerciseService;
        private readonly IProductService _productService;
        private readonly IUserProductService _userProductService;
        private readonly IUserExerciseService _userExerciseService;
        private readonly IDayRepository _dayRepository;


        public DataInitializer(IUserService userService, IRoleRepository roleRepository, IExerciseService exerciseService, IProductService productService,
             IUserProductService userProductService, IUserExerciseService userExerciseService, IDayRepository dayRepository)
        {
            _userService = userService;
            _roleRepository = roleRepository;
            _exerciseService = exerciseService;
            _productService = productService;
            _userProductService = userProductService;
            _userExerciseService = userExerciseService;
            _dayRepository = dayRepository;
        }

        public async Task SeedAsync()
        {
            var users = await _userService.GetAllAsync();
            if (users.Any())
            {
                return;
            }

            var role = await _roleRepository.GetAsync("User");
            var day = await _dayRepository.GetAsync("Monday");

            for (var i = 1; i <= 10; i++)
            {
                await _userService.RegisterAsync($"user{i}", $"user{i}@email.com", "secret", role.Name);
                await _userProductService.AddAsync(i, i, 100);
                await _userExerciseService.AddAsync(i, i, 100, 4, 12, day.Name);

            }

            role = await _roleRepository.GetAsync("Admin");

            for (var i = 1; i <= 2; i++)
            {
                await _userService.RegisterAsync($"admin{i}", $"admin{i}@email.com", "secret", role.Name);
            }
        }
    }
}
