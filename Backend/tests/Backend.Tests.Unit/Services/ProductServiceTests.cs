using AutoMapper;
using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Exceptions;
using Backend.Core.Helpers;
using Backend.Infrastructure.Mappers;
using Backend.Core.Repositories;
using Backend.Infrastructure.Services;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Backend.Infrastructure.Repositories;
using AutoFixture;
using Backend.Core.Exceptions;
using Backend.Infrastructure.Extensions;

namespace Backend.Tests.Unit.Services
{
    public class ProductServiceTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ProductService _sut;
        private readonly IFixture _fixture;

        public ProductServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _mapper = AutoMapperConfig.Initialize();
            _sut = new ProductService(_productRepository, _mapper);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetAsyncById_ShouldReturnProductDetailsDto(int id)
        {
            // Arrange
            var product = _fixture.Build<Product>()
                .With(p => p.Id, id)
                .Create();
            _productRepository.GetAsync(id).Returns(product);

            // Act
            var dto = await _sut.GetAsync(id);

            // Assert
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(product.Id);
            dto.Name.ShouldBe(product.Name);
            dto.ShouldBeOfType(typeof(ProductDto));
            await _productRepository.Received(1).GetAsync(product.Id);
        }

        [Theory]
        [InlineData("product1")]
        [InlineData("product2")]
        public async Task GetAsyncByName_ShouldReturnProductDetailsDto(string name)
        {
            // Arrange
            var product = _fixture.Build<Product>()
                .With(p => p.Name, name)
                .Create();
            _productRepository.GetAsync(name).Returns(product);

            // Act
            var dto = await _sut.GetAsync(name);

            // Assert
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(product.Id);
            dto.Name.ShouldBe(product.Name);
            dto.ShouldBeOfType(typeof(ProductDto));
            await _productRepository.Received(1).GetAsync(Arg.Is(name));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedListOfAllProductsDto_WhenParametersAreNull()
        {
            // Arrange
            var products = _fixture.Build<Product>()
                          .CreateMany();

            var paginationQuery = new PaginationQuery();
            var page = new PagedList<Product>(products.ToList(), products.ToList().Count(), 1, 10);
            _productRepository.GetAllAsync(null, null, paginationQuery).Returns(page);


            // Act
            var dto = await _sut.GetAllAsync(null, null, paginationQuery);

            // Assert
            dto.ShouldNotBeNull();
            dto.ShouldBeOfType(typeof(PageResultDto<ProductDto>));
            dto.Items.Count.ShouldBe(products.ToList().Count);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewProduct()
        {
            // Arrange
            var product = _fixture.Build<Product>()
                .Create();

            // Act
            var id = await _sut.AddAsync(product.Name, product.Calories, product.Proteins,
                product.Carbohydrates, product.Fats, product.CategoryOfProduct.Name.ToString());

            // Assert
            await _productRepository.Received(1).AddAsync(Arg.Is<Product>(p => 
            p.Id == id &&
            p.Name == product.Name &&
            p.Proteins == product.Proteins &&
            p.Carbohydrates == product.Carbohydrates &&
            p.Fats == product.Fats));
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenNameIsInvalid()
        {
            var name = "";
            var calories = 100;
            var proteins = 10;
            var carbo = 20;
            var fats = 5;
            var category = "Fruits";

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(name, calories, proteins,
                carbo, fats, category));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidNameException));
            exception.Message.ShouldBe("Name cannot be empty.");
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenCaloriesAreLowerThanZero()
        {
            var product = _fixture.Build<Product>()
               .Create();

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(product.Name, -10, product.Proteins,
                product.Carbohydrates, product.Fats, product.CategoryOfProduct.Name.ToString()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidCaloriesException));
            exception.Message.ShouldBe("Calories cannot be less than 0.");
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenProteinsAreInvalid()
        {
            var name = "name";
            var calories = 100;
            var proteins = -10;
            var carbo = 20;
            var fats = 5;
            var category = "Fruits";

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(name, calories, proteins,
                carbo, fats, category));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidProteinsException));
            exception.Message.ShouldBe("Proteins cannot be less than 0.");
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenCarbohydratesAreInvalid()
        {
            var name = "name";
            var calories = 100;
            var proteins = 10;
            var carbo = -20;
            var fats = 5;
            var category = "Fruits";

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(name, calories, proteins,
                carbo, fats, category));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidCarbohydratesException));
            exception.Message.ShouldBe("Carbohydrates cannot be less than 0.");
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenFatsAreInvalid()
        {
            var name = "name";
            var calories = 100;
            var proteins = 10;
            var carbo = 20;
            var fats = -5;
            var category = "Fruits";

            var exception = await Record.ExceptionAsync(() => _sut.AddAsync(name, calories, proteins,
                carbo, fats, category));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(InvalidFatsException));
            exception.Message.ShouldBe("Fats cannot be less than 0.");
        }


        [Theory]
        [InlineData("product1")]
        [InlineData("product2")]
        public async Task AddAsync_ShouldThrowException_WhenProductNameIsNotUnique(string name)
        {
            var product = _fixture.Build<Product>()
                .With(p => p.Name, name)
                .Create();
            _productRepository.GetAsync(name).Returns(product);

            var exception = await Record.ExceptionAsync(() =>
                _sut.AddAsync(product.Name, product.Calories, product.Proteins,
                product.Carbohydrates, product.Fats, product.CategoryOfProduct.Name.ToString()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(NameInUseException));
            exception.Message.ShouldBe($"Product with name: '{name}' already exists.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsync_ShouldDeleteProduct_IfProductExists(int id)
        {
            var product = _fixture.Build<Product>()
                .With(p => p.Id, id)
                .Create();
            _productRepository.GetAsync(id).Returns(product);

            await _sut.DeleteAsync(id);

            await _productRepository.Received(1).DeleteAsync(Arg.Is(product));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsync_ShouldThrowException_WhenProductDoesNotExist(int id)
        {
            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ProductNotFoundException));
            exception.Message.ShouldBe($"Product with id: '{id}' was not found.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExistsAndDataIsValid()
        {
            var product = _fixture.Build<Product>()
                .Create();
            _productRepository.GetAsync(product.Id).Returns(product);

            await _sut.UpdateAsync(product.Id, product.Name, product.Calories, product.Proteins,
                product.Carbohydrates, product.Fats, product.CategoryOfProduct.Name.ToString());

            await _productRepository.Received(1).UpdateAsync(Arg.Is<Product>(p => 
            p.Id == product.Id &&
            p.Name == product.Name &&
            p.Proteins == product.Proteins &&
            p.Carbohydrates == product.Carbohydrates &&
            p.Fats == product.Fats));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(1, "blabla", 100, 10, 10, 10, "Meat"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(ProductNotFoundException));
            exception.Message.ShouldBe("Product with id: '1' was not found.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenProductExistsButNameIsNotUnique()
        {
            var existing = "randomProduct";
            var product = _fixture.Build<Product>()
               .Create();
            _productRepository.GetAsync(product.Id).Returns(product);

            _productRepository.AnyAsync(p => p.Name == existing).ReturnsForAnyArgs(true);

            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(product.Id, existing, 
                product.Calories, product.Proteins, product.Carbohydrates, product.Fats, product.CategoryOfProduct.Name.ToString()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(NameInUseException));
            exception.Message.ShouldBe($"Product with name: '{existing}' already exists.");
        }

    }
}
