using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NFluent;
using Xunit;

namespace MVCAsyncAwait.Tests
{
    public class IntegrationTest
    {
        private readonly HttpClient _client;
        private DbContextOptions<MovieContext> _dataBaseOptions;

        public IntegrationTest()
        {
            var factory =new WebApplicationFactory<Startup>().WithWebHostBuilder(build => 
                build.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(MovieContext));
                    services.AddDbContext<MovieContext>(options => options.UseInMemoryDatabase("databaseTest"));
                }));
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("/Home/Index"), InlineData("/Home/AddMovie"), InlineData("/Home/Privacy")]
        public async Task Should_return_content_when_calling_valid_url(string url)
        {
            var response = await _client.GetAsync(url);
            
            Check.That(response).IsNotNull();
            Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
            Check.That(await response.Content.ReadAsStringAsync()).IsNotEmpty();

        }

        [Fact]
        public async Task Should_have_on_movie_on_the_list_when_movie_was_add()
        {
            _dataBaseOptions = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "databaseTest")
                .Options;

            await using (var context = new MovieContext(_dataBaseOptions))
            {
                var service = new MovieService(context);
                await service.Add("Casablanca");
                context.SaveChanges();
            }

            await using (var context = new MovieContext(_dataBaseOptions))
            {
                Assert.Equal(1, context.Movies.Count());
                Assert.Equal("Casablanca", context.Movies.Single().Name);
            }
        }

        [Fact]
        public async Task Should_update_the_movie_when_the_name_was_modified()
        {
            _dataBaseOptions = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "databaseTest2")
                .Options;
            await using (var context = new MovieContext(_dataBaseOptions))
            {
                var service = new MovieService(context);
                await service.Add("Casablanca");
                context.SaveChanges();
            }

            await using (var context = new MovieContext(_dataBaseOptions))
            {
                var service = new MovieService(context);
                var movieToUpdate = (await service.Find("Casablanca")).FirstOrDefault();
                movieToUpdate.Name = "Titanic";
                await service.Update(movieToUpdate);
            }

            await using (var context = new MovieContext(_dataBaseOptions))
            {
                Assert.Equal(1, context.Movies.Count());
                Assert.Equal("Titanic", context.Movies.Single().Name);
            }
        }

        [Fact]
        public async Task Should_have_zero_movie_when_movie_deleted()
        {
            _dataBaseOptions = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "databaseTest3")
                .Options;
            await using (var context = new MovieContext(_dataBaseOptions))
            {
                var service = new MovieService(context);
                await service.Add("Casablanca");
                context.SaveChanges();
            }

            await using (var context = new MovieContext(_dataBaseOptions))
            {
                var service = new MovieService(context);
                var movieToDelete = (await service.Find("Casablanca")).FirstOrDefault();
                await service.Delete(movieToDelete?.MovieId);
            }

            await using (var context = new MovieContext(_dataBaseOptions))
            {
                Assert.Equal(0, context.Movies.Count());
            }
        }
    }
}
