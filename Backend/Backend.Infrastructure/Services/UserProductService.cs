using AutoMapper;
using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class UserProductService : IUserProductService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserProductRepository _userProductRepository;
        private readonly IMapper _mapper;

        public UserProductService(IUserRepository userRepository, IProductRepository productRepository, IUserProductRepository userProductRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _userProductRepository = userProductRepository;
            _mapper = mapper;
        }

        public async Task<UserProductDto> GetAsync(int userProductId)
        {
            var userProduct = await _userProductRepository.GetAsync(userProductId);

            return _mapper.Map<UserProductDto>(userProduct);
        }

        public async Task<UserProductDto> GetAsync(int userId, int productId)
        {
            var userProduct = await _userProductRepository.GetAsync(userId, productId);

            return _mapper.Map<UserProductDto>(userProduct);
        }

        public async Task<UserDetailsDto> GetAllUserProducts(int userId)
        {
            var userProduct = await _userProductRepository.GetAllProductsForUserAsync(userId);

            return _mapper.Map<UserDetailsDto>(userProduct);
        }

        public async Task<IEnumerable<UserProductDetailsDto>> GetAllAsync()
        {
            var userProducts = await _userProductRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserProductDetailsDto>>(userProducts);
        }

        public async Task<IEnumerable<UserProductDetailsDto>> GetAllUserProductsFromDay(int userId, string date)
        {
            DateTime dateTime = DateTime.Parse(date);
            DateTime dateOnly = dateTime.Date;
            var userProducts = await _userProductRepository.GetAllProductsFromDayAsync(userId, dateOnly);

            return _mapper.Map<IEnumerable<UserProductDetailsDto>>(userProducts);
        }

        public async Task AddAsync(int userId, int productId, double weight)
        {
          
            DateTime dateTime = DateTime.Today;
            DateTime dateOnly = dateTime.Date;
            var userProduct = await _userProductRepository.GetAsync(userId, productId);
            if (userProduct != null && userProduct.AddedAt == dateOnly)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"User with id: {userProduct.UserId} already has product with id: {userProduct.ProductId}");
            }

            var user = await _userRepository.GetAsync(userId);
            var product = await _productRepository.GetAsync(productId);

            user.AddProduct(user, product, weight);
       
            await _userRepository.UpdateAsync(user);      
        }

        public async Task DeleteAsync(int userProductId)
        {

            var userProduct = await _userProductRepository.GetAsync(userProductId);
            if (userProduct == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    $"Product with id: {userProduct.ProductId} for user with id: {userProduct.UserId} was not found");
            }

            await _userProductRepository.DeleteAsync(userProduct);
        }

        public async Task UpdateAsync(int userId, int productId, double weight)
        {
            var userProduct = await _userProductRepository.GetAsync(userId, productId);
            if (userProduct == null)
            {
                throw new ServiceException(Exceptions.ErrorCodes.ObjectNotFound,
                    "Product does not exist.");
            }

            var product = await _productRepository.GetAsync(productId);

            userProduct.SetWeight(weight);
            userProduct.AdjustCalories(weight, product);

            await _userProductRepository.UpdateAsync(userProduct);
        }

        
    }
}
