using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.EF;
using Backend.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Integration.Controllers
{
    public class ExercisesControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ExercisesControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_ShouldReturnExerciseDto_WhenExerciseExists()
        {
            var client = FreshClient();

            await client.AuthenticateAsync();

            var addExercise = new AddExercise
            {
                Name = "testExercise",
                PartOfBody = PartOfBodyId.Chest.ToString()
            };

           var createdExercise = await client.CreatePostAsync("api/exercises", addExercise);

            var response = await client.GetAsync($"api/exercises/{createdExercise.Id}");

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var exercise = await response.ReadAsString<ExerciseDto>();

            exercise.Id.ShouldBe(createdExercise.Id);
            exercise.Name.ShouldBe(createdExercise.Name);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenExerciseDoesNotExist()
        {
            var client = FreshClient();

            var response = await client.GetAsync($"/api/exercises/1");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetByName_ShouldReturnOk_WhenExerciseExists()
        {
            var client = FreshClient();
            await client.AuthenticateAsync();

            var addExercise = new AddExercise
            {
                Name = "testExercise",
                PartOfBody = PartOfBodyId.Chest.ToString()
            };

            await client.PostAsJsonAsync($"api/exercises", addExercise);

            var response = await client.GetAsync($"api/exercises/{addExercise.Name}");

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var exercise = await response.ReadAsString<ExerciseDto>();
            exercise.Name.ShouldBe(addExercise.Name);
            exercise.PartOfBody.ShouldBe(addExercise.PartOfBody);
        }

        [Fact]
        public async Task GetByName_ShouldReturnNotFound_WhenExerciseDoesNotExist()
        {
            var client = FreshClient();
            var name = "random";

            var response = await client.GetAsync($"api/exercises/{name}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("api/exercises?pageNumber=1&pageSize=10")]
        [InlineData("api/exercises?name=testExercise&pageNumber=1&pageSize=10")]
        [InlineData("api/exercises?name=testExercise&partOfBody=Chest&pageNumber=1&pageSize=10")]
        public async Task GetAll_ShouldReturnIEnumerableExerciseDto(string query)
        {
            var client = FreshClient();

            await client.AuthenticateAsync();

            var addExercise1 = new AddExercise
            {
                Name = "testExercise",
                PartOfBody = PartOfBodyId.Chest.ToString()
            };

            var addExercise2 = new AddExercise
            {
                Name = "testExercise2",
                PartOfBody = PartOfBodyId.Chest.ToString()
            };

            var createdExercise1 = await client.CreatePostAsync("api/exercises", addExercise1);
            var createdExercise2 = await client.CreatePostAsync("api/exercises", addExercise2);

            var response = await client.GetAsync(query);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var pagination = response.Headers.GetValues("X-Pagination");

            pagination.ShouldNotBeNull();

            var exercisesDto = await response.ReadAsString<IEnumerable<ExerciseDto>>();

            exercisesDto.ShouldNotBeNull();
            exercisesDto.Count().ShouldBe(2);
        }

        [Fact]
        public async Task Post_ShouldReturnCrated_WhenUserIsAdminAndDataIsValid()
        {
            var client = FreshClient();

            await client.AuthenticateAsync();

            var addExercise = new AddExercise
            {
                Name = "testExercise",
                PartOfBody = PartOfBodyId.Chest.ToString()
            };

            var response = await client.PostAsJsonAsync("api/exercises", addExercise);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var createdExercise = await response.ReadAsString<AddExercise>();

            createdExercise.Name.ShouldBe(addExercise.Name);
            createdExercise.PartOfBody.ShouldBe(addExercise.PartOfBody);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenUserIsAdminAndDataIsNotValid()
        {
            var client = FreshClient();
            await client.AuthenticateAsync();

            var addExercise = new AddExercise
            {
                PartOfBody = PartOfBodyId.Chest.ToString()
            };

            var response = await client.PostAsJsonAsync("api/exercises", addExercise);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
        {
            var client = FreshClient();
            var addExercise = new AddExercise
            {
                Name = "testExercise",
                PartOfBody = PartOfBodyId.Chest.ToString()
            };

            var response = await client.PostAsJsonAsync("api/exercises", addExercise);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Post_shouldreturnforbidden_whenuserisauthorizedbutisnotadmin()
        {
            var client = FreshClient();
            await client.AuthenticateUserAsync();

            var response = await client.PostAsJsonAsync("api/exercises", new AddExercise { });

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenUserIsAdminAndExerciseExists()
        {
            var client = FreshClient();

            await client.AuthenticateAsync();

            var addExercise = new AddExercise
            {
                Name = "testExercise",
                PartOfBody = PartOfBodyId.Chest.ToString()
            };

            var createdExercise = await client.CreatePostAsync("api/exercises", addExercise);

            var response = await client.DeleteAsJsonAsync("api/exercises", new DeleteExercise
            {
                ExerciseId = createdExercise.Id
            });

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenUserIsAdminAndDataIsNotValid()
        {
            var client = FreshClient();
            await client.AuthenticateAsync();

            var response = await client.DeleteAsJsonAsync("api/exercises", new DeleteExercise
            {
            });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
        {
            var client = FreshClient();
            var response = await client.DeleteAsJsonAsync("api/exercises", new DeleteExercise
            {
                ExerciseId = 5
            });

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_ShouldReturnForbidden_WhenUserIsAuthorizedButIsNotAdmin()
        {
            var client = FreshClient();

            await client.AuthenticateUserAsync();

            var response = await client.DeleteAsJsonAsync("api/exercises", new DeleteExercise
            {
                ExerciseId = 5
            });

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenUserIsAdminAndDataIsValid()
        {
            var client = FreshClient();

            await client.AuthenticateAsync();

            var addExercise = new AddExercise
            {
                Name = "testExercise",
                PartOfBody = PartOfBodyId.Chest.ToString()
            };

            var createdExercise = await client.CreatePostAsync("api/exercises", addExercise);

            var updateExercise = new UpdateExercise
            {
                ExerciseId = createdExercise.Id,
                Name = createdExercise.Name + "Updated",
                PartOfBody = createdExercise.PartOfBody
            };

            var response = await client.PutAsJsonAsync("api/exercises", updateExercise);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var exerciseResponse = await client.GetAsync($"api/exercises/{updateExercise.ExerciseId}");

            var updatedExercise = await exerciseResponse.ReadAsString<ExerciseDto>();

            updatedExercise.Name.ShouldBe(updateExercise.Name);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenUserIsAdminAndDataIsNotValid()
        {
            var client = FreshClient();
            await client.AuthenticateAsync();

            var response = await client.PutAsJsonAsync("api/exercises", new UpdateExercise
            {
                ExerciseId = 10,
                Name = "",
                PartOfBody = PartOfBodyId.Chest.ToString()
            });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
        {
            var client = FreshClient();
            var response = await client.PostAsJsonAsync("api/exercises", new UpdateExercise
            {
                ExerciseId = 10,
                Name = "Fake",
                PartOfBody = PartOfBodyId.Chest.ToString()
            });

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Put_ShouldReturnForbidden_WhenUserIsAuthorizedButIsNotAdmin()
        {
            var client = FreshClient();
            await client.AuthenticateUserAsync();

            var response = await client.PutAsJsonAsync("api/exercises", new UpdateExercise
            {
                ExerciseId = 10,
                Name = "Fake",
                PartOfBody = PartOfBodyId.Chest.ToString()
            });

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
                        db.Exercises.RemoveRange(db.Exercises);
                        db.SaveChanges();
                    }
                });
            }).CreateClient();

            return client;
        }
    }
}
