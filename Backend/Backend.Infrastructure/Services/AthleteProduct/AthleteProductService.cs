using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class AthleteProductService : IAthleteProductService
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IProductRepository _productRepository;

        public AthleteProductService(IAthleteRepository athleteRepository, IProductRepository productRepository)
        {
            _athleteRepository = athleteRepository;
            _productRepository = productRepository;
        }

        public async Task AddAsync(int userId, int productId, double weight)
        {
            var athlete = await _athleteRepository.GetProductsAsync(userId);
            if (athlete == null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Athlete with user id: {userId} was not found.");
            }
            var product = await _productRepository.GetAsync(productId);
            if (product == null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Product with id: {productId} was not found.");
            }

            if (athlete.AthleteProducts.Any(ap => ap.ProductId == productId))
            {
                throw new ServiceException(ErrorCodes.ObjectAlreadyAdded, $"Product with id: {productId} already added today");
            }

            athlete.AthleteProducts.Add(AthleteProduct.Create(athlete, product, weight));

            await _athleteRepository.UpdateAsync(athlete);
        }

        public async Task DeleteAsync(int userId, int productId)
        {
            var athlete = await _athleteRepository.GetProductsAsync(userId);
            if (athlete is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Athlete with userId: {userId} was not found.");
            }

            var product = athlete.AthleteProducts.SingleOrDefault(ap => ap.ProductId == productId);
            if (product is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Product with id {productId} for athlete with userId {userId} was not found");
            }

            athlete.AthleteProducts.Remove(product);

            await _athleteRepository.UpdateAsync(athlete);
        }
    }
}
