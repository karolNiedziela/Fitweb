using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Core.Helpers;
using Backend.Core.Repositories;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Repositories;
using Backend.Tests.Integration.Helpers;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Integration.Repositories
{
    public class ProductRepositoryTests
    {
        private FitwebContext _fixture;

        private readonly IProductRepository _sut;

        public ProductRepositoryTests()
        {
            var builder = new DbContextOptionsBuilder<FitwebContext>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=fitweb_{Guid.NewGuid()};Trusted_Connection=True;" +
                $"MultipleActiveResultSets=true");

            _fixture = new FitwebContext(builder.Options);
            _fixture.Database.Migrate();
            _sut = new ProductRepository(_fixture);
        }

        [Fact]
        public async Task GetAsyncById_ShouldReturnProduct_WhenProductExists()
        {
            var product = new Product("testProduct", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString()));

            await _sut.AddAsync(product);

            var returnProduct =  await _sut.GetAsync(product.Id);

            returnProduct.Id.ShouldBe(product.Id);
            returnProduct.Name.ShouldBe(product.Name);
            returnProduct.Calories.ShouldBe(product.Calories);
            returnProduct.Proteins.ShouldBe(product.Proteins);
            returnProduct.Carbohydrates.ShouldBe(product.Carbohydrates);
            returnProduct.Fats.ShouldBe(product.Fats);
        }

        [Fact]
        public async Task GetAsyncByName_ShouldReturnProduct_WhenProductExists()
        {
            var product = new Product("testProduct", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString()));

            await _sut.AddAsync(product);

            var returnProduct = await _sut.GetAsync(product.Name);

            returnProduct.Id.ShouldBe(product.Id);
            returnProduct.Name.ShouldBe(product.Name);
            returnProduct.Calories.ShouldBe(product.Calories);
            returnProduct.Proteins.ShouldBe(product.Proteins);
            returnProduct.Carbohydrates.ShouldBe(product.Carbohydrates);
            returnProduct.Fats.ShouldBe(product.Fats);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts_WhenParametersAreOmitted()
        {
            var products = new List<Product>
            {
                new Product("testProduct", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString())),

                new Product("testProduct2", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString()))
            };

            await _sut.AddRangeAsync(products);

            var returnProducts = await _sut.GetAllAsync(null, null, new PaginationQuery());

            returnProducts.Items.Count.ShouldBe(2);
            returnProducts.ShouldBeOfType(typeof(PagedList<Product>));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProductsFromGivenCategory_WhenCategoryIsGiven()
        {
            var products = new List<Product>
            {
                new Product("testProduct", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString())),

                new Product("testProduct2", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString())),

                new Product("testProduct3", 100, 20, 30, 15, 
                CategoryOfProduct.GetCategory(CategoryOfProductId.Drinks.ToString()))
            };

            await _sut.AddRangeAsync(products);

            var returnProducts = await _sut.GetAllAsync(null, CategoryOfProductId.Drinks.ToString(), new PaginationQuery());

            returnProducts.Items.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProductsWhichCotainGivenName_WhenNameIsGiven()
        {
            var products = new List<Product>
            {
                new Product("testProduct", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString())),

                new Product("testProduct2", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString())),

                new Product("testProduct3", 100, 20, 30, 15,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Drinks.ToString()))
            };

            await _sut.AddRangeAsync(products);

            var returnProducts = await _sut.GetAllAsync("test", null, new PaginationQuery());

            returnProducts.Items.Count.ShouldBe(3);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProduct_WhenProductIsValid()
        {
            var product = new Product("testProduct", 100, 10, 50, 10, 
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString()));

            await _sut.AddAsync(product);

            _fixture.Products.Count(p => p.Name == product.Name).ShouldBe(1);
        }

        [Fact]
        public async Task AddRangeAsync_ShouldAddListOfProduct_WhenProductsAreValid() 
        {
            var products = new List<Product>
            {
                new Product("testProduct1", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString())),

                new Product("testProduct2", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString()))
            };

            await _sut.AddRangeAsync(products);

             _fixture.Products.Count().ShouldBe(2);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct_WhenProductIsValid()
        {
            var product = new Product("testProduct", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString()));

            await _sut.AddAsync(product);

            await _sut.DeleteAsync(product);

            _fixture.Products.Count().ShouldBe(0);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenProductIsValid()
        {
            var product = new Product("testProduct", 100, 10, 50, 10,
                CategoryOfProduct.GetCategory(CategoryOfProductId.Bread.ToString()));

            await _sut.AddAsync(product);

            product.SetFats(30);
            product.SetCalories(15);

            await _sut.UpdateAsync(product);

            var returnProduct = await _fixture.Products.FindAsync(product.Id);

            returnProduct.Fats.ShouldBe(30);
            returnProduct.Calories.ShouldBe(15);
        }
    }
}
