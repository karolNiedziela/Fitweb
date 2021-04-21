using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class AthleteProductService : IAthleteProductService
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAthleteDietStatsService _athleteDietStatsService;

        public AthleteProductService(IAthleteRepository athleteRepository, IProductRepository productRepository,
            IAthleteDietStatsService athleteDietStatsService)
        {
            _athleteRepository = athleteRepository;
            _productRepository = productRepository;
            _athleteDietStatsService = athleteDietStatsService;
        }

        public async Task AddAsync(int userId, int productId, double weight)
        {
            var athlete = await _athleteRepository.FindByCondition(condition: a => a.UserId == userId,
                include: source => source.Include(a => a.AthleteProducts.Where(ap => ap.ProductId == productId)));

            if (athlete is null)
            {
                throw new AthleteNotFoundException(userId);
            }

            var product = await _productRepository.GetOrFailAsync(productId);

            athlete.AthleteProducts.Add(AthleteProduct.Create(athlete, product, weight));

            await UpdateDietStats(athlete, userId, product, weight, '+');
        }

        public async Task DeleteAsync(int userId, int productId, int athleteProductId)
        {
            var athlete = await _athleteRepository.FindByCondition(condition: a => a.UserId == userId,
                include: source => source.Include(a => a.AthleteProducts.Where(ap => ap.Id == athleteProductId))
                    .ThenInclude(ap => ap.Product));

            if (athlete is null)
            {
                throw new AthleteNotFoundException(userId);
            }

            var athleteProduct = athlete.AthleteProducts.SingleOrDefault(ap => ap.Id == athleteProductId);
            if (athleteProduct is null)
            {
                throw new ProductForAthleteNotFoundException(userId, productId);
            }

            await _athleteRepository.RemoveProductAsync(athlete, athleteProduct);

            await UpdateDietStats(athlete, userId, athleteProduct.Product, athleteProduct.Weight, '-');       
        }

        private async Task UpdateDietStats(Athlete athlete, int userId , Product product, double weight, char sign)
        {
            athlete.AthleteDietStats.Add(await _athleteDietStatsService.AddDietStatsAsync(userId, product, weight, sign));

            await _athleteRepository.UpdateAsync(athlete);
        }
    }
}
