using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Core.Helpers;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.EF;
using Backend.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Integration.Controllers
{
    public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ProductsControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("testProduct1", 100, 10, 10, 10, "Meat")]
        [InlineData("testProduct2", 500, 25, 70, 5, "Meat")]
        public async Task Get_ShouldReturnProductDetailsDto_WhenProductExistsWithGivenId(string name, double calories,
            double proteins, double carbohydrates, double fats, string categoryName)
        {
            var client = FreshClient();

            await client.AuthenticateAsync();
            var addProduct = new AddProduct
            {
                Name = name,
                Calories = calories,
                Proteins = proteins,
                Carbohydrates = carbohydrates,
                Fats = fats,
                CategoryName = categoryName
            };
            var createdProduct = await client.CreatePostAsync("/api/products", addProduct);

            var response = await client.GetAsync($"api/products/{createdProduct.Id}");

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var returnedProduct = await response.ReadAsString<ProductDetailsDto>();

            returnedProduct.Id.ShouldBe(createdProduct.Id);
            returnedProduct.Name.ShouldBe(createdProduct.Name);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenProductDoesNotExistWithGivenId()
        {
            var client = FreshClient();

            var response = await client.GetAsync($"api/products/{1}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAll_ShouldReturnIEnumerableProductDetailsDtoWithGivenPagination()
        {
            var client = FreshClient();

            await client.AuthenticateAsync();
            var addProduct = new AddProduct
            {
                Name = "testProduct",
                Calories = 100,
                Proteins = 10,
                Carbohydrates = 15,
                Fats = 10,
                CategoryName = "Meat"
            };
            await client.CreatePostAsync("/api/products", addProduct);

            var response = await client.GetAsync($"api/products?pageNumber=1&pageSize=10");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var pagination = response.Headers.GetValues("X-Pagination");

            pagination.ShouldNotBeNull();

            var products = await response.ReadAsString<IEnumerable<ProductDetailsDto>>();
            products.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Post_ShouldReturnCreatedResponse_WhenUserIsAdminAndDataIsValid()
        {
            var client = FreshClient();

            await client.AuthenticateAsync();
            var addProduct = new AddProduct
            {
                Name = "testProduct",
                Calories = 100,
                Proteins = 10,
                Carbohydrates = 15,
                Fats = 10,
                CategoryName = "Meat"
            };
            var response = await client.PostAsJsonAsync("/api/products", addProduct);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var createdProduct = await response.ReadAsString<AddProduct>();

            createdProduct.Name.ShouldBe(addProduct.Name);
            createdProduct.Calories.ShouldBe(addProduct.Calories);
            createdProduct.Proteins.ShouldBe(addProduct.Proteins);
            createdProduct.Carbohydrates.ShouldBe(addProduct.Carbohydrates);
            createdProduct.Fats.ShouldBe(addProduct.Fats);
        }

        [Fact]
        public async Task Post_ShouldReturnUnauthorized_WhenUserIsNotAdmin()
        {
            var addProduct = new AddProduct
            {
                Name = "testProduct",
                Calories = 100,
                Proteins = 10,
                Carbohydrates = 15,
                Fats = 10,
                CategoryName = "Meat"
            };

            var response = await _client.PostAsJsonAsync("/api/products", addProduct);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenDataIsNotValidAndUserIsAdmin()
        {
            await _client.AuthenticateAsync();

            var addProduct = new AddProduct
            {
                Name = "testProduct",
                Calories = -100,
                CategoryName = CategoryOfProductId.Meat.ToString()
            };

            var response = await _client.PostAsJsonAsync("/api/products", addProduct);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenUserIsAuthorizedButIsNotAdmin()
        {
            await _client.AuthenticateUserAsync();

            var addProduct = new AddProduct
            {
                Name = "testProduct",
                Calories = -100,
                CategoryName = CategoryOfProductId.Meat.ToString()
            };

            var response = await _client.PostAsJsonAsync("api/products", addProduct);

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Post_ShouldReturnForbidden_WhenUserIsAuthorizedButIsNotAdmin()
        {
            await _client.AuthenticateUserAsync();

            var response = await _client.PostAsJsonAsync("api/products", new AddProduct { });

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenProductExistsAndUserIsAdmin()
        {
            var client = FreshClient();

            await client.AuthenticateAsync();
            var addProduct = new AddProduct
            {
                Name = "testProduct",
                Calories = 100,
                Proteins = 10,
                Carbohydrates = 15,
                Fats = 10,
                CategoryName = "Meat"
            };

            var createdProduct = await client.CreatePostAsync("/api/products", addProduct);

            var response = await client.DeleteAsJsonAsync("/api/products", new DeleteProduct
            {
                ProductId = createdProduct.Id
            });

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenUserIsNotAdmin()
        {
            var response = await _client.DeleteAsJsonAsync("/api/products", new DeleteProduct
            {
                ProductId = 1
            });

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenUserIsAdminButDataIsNotValid()
        {
            await _client.AuthenticateAsync();
            var response = await _client.DeleteAsJsonAsync("/api/products", new DeleteProduct
            {
                ProductId = 10
            });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ShouldReturnForbidden_WhenUserIsAuthorizedButIsNotAdmin()
        {
            await _client.AuthenticateUserAsync();

            var response = await _client.DeleteAsJsonAsync("api/products", new DeleteProduct { });

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenUserIsAdminAndDataIsValid()
        {
            var client = FreshClient();

            await client.AuthenticateAsync();
            var addProduct = new AddProduct
            {
                Name = "testProduct",
                Calories = 100,
                Proteins = 10,
                Carbohydrates = 15,
                Fats = 10,
                CategoryName = "Meat"
            };

            var createdProduct = await client.CreatePostAsync("/api/products", addProduct);

            var updateProduct = new UpdateProduct
            {
                ProductId = createdProduct.Id,
                Name = createdProduct.Name,
                Calories = createdProduct.Calories + 20,
                Proteins = createdProduct.Proteins,
                Carbohydrates = createdProduct.Carbohydrates,
                Fats = createdProduct.Fats,
                CategoryName = createdProduct.CategoryName
            };

            var response = await client.PutAsJsonAsync("/api/products", updateProduct);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Put_ShouldReturnUnauthorized_WhenUserIsNotAdmin()
        {
            var response = await _client.PutAsJsonAsync("/api/products", new UpdateProduct { });

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenUserIsAdminButDataIsNotValid()
        {
            await _client.AuthenticateAsync();
            var response = await _client.DeleteAsJsonAsync("/api/products", new UpdateProduct { });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_ShouldReturnForbidden_WhenUserIsAuthorizedButIsNotAdmin()
        {
            await _client.AuthenticateUserAsync();

            var response = await _client.DeleteAsJsonAsync("api/products", new UpdateProduct { });

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }


        private HttpClient FreshClient()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;

                        var db = scopedServices.GetRequiredService<FitwebContext>();
                        db.Products.RemoveRange(db.Products);
                        db.SaveChanges();
                    }
                });
            }).CreateClient();

            return client;
        }
    }
}
