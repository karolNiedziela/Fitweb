﻿using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Repositories;
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

        public AthleteProductService(IAthleteRepository athleteRepository, IProductRepository productRepository)
        {
            _athleteRepository = athleteRepository;
            _productRepository = productRepository;
        }

        public async Task AddAsync(int userId, int productId, double weight)
        {
            var athlete = await _athleteRepository.FindByCondition(condition: a => a.UserId == userId,
                include: source => source.Include(a => a.AthleteProducts.Where(ap => ap.ProductId == productId))
                                                    .ThenInclude(ap => ap.Product)
                                                        .ThenInclude(p => p.CategoryOfProduct));

            if (athlete is null)
            {
                throw new ServiceException(ErrorCodes.AthleteNotFound, $"Athlete with user id: {userId} was not found.");
            }

            if (athlete.AthleteProducts.Any(ap => ap.ProductId == productId && 
                ap.DateUpdated.ToShortDateString() == DateTime.Today.ToShortDateString()))
            {
                throw new ServiceException(ErrorCodes.ObjectAlreadyAdded, $"Product with id: {productId} already added today");
            }

            var product = await _productRepository.GetAsync(productId);
            if (product is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Product with id: {productId} was not found.");
            }

            athlete.AthleteProducts.Add(AthleteProduct.Create(athlete, product, weight));

            await _athleteRepository.UpdateAsync(athlete);
        }

        public async Task DeleteAsync(int userId, int productId)
        {
            var athlete = await _athleteRepository.FindByCondition(condition: a => a.UserId == userId,
                include: source => source.Include(a => a.AthleteProducts)
                                                    .ThenInclude(ap => ap.Product)
                                                        .ThenInclude(p => p.CategoryOfProduct));
            if (athlete is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Athlete with user id: {userId} was not found.");
            }

            var product = athlete.AthleteProducts.SingleOrDefault(ap => ap.ProductId == productId);
            if (product is null)
            {
                throw new ServiceException(ErrorCodes.ObjectNotFound, $"Product with id {productId} for athlete with " +
                    $"user id {userId} was not found.");
            }

            athlete.AthleteProducts.Remove(product);

            await _athleteRepository.UpdateAsync(athlete);
        }
    }
}