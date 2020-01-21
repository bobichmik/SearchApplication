using Domain.Core.Models;
using Domain.Core.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryTests
{
    public class RepositoriesTests
    {
        [Fact]
        public async Task AddTest()
        {
            var repository = GetInMemorySearchResultsRepository();
            var entity = new SearchResultModel { SearchTerm = "testTerm", Title = "testTitle", Url = "testUrl", Id = 1 };

            await repository.Add(entity);

            var firstEntity = repository.GetAll().FirstOrDefault();
            Assert.Equal(entity, firstEntity);
        }

        [Fact]
        public async Task AddRangeTest()
        {
            var repository = GetInMemorySearchResultsRepository();
            var entities = new List<SearchResultModel> {
                new SearchResultModel{ SearchTerm = "testTerm", Title = "testTitle", Url = "testUrl", Id = 1 },
                new SearchResultModel{ SearchTerm = "testTerm2", Title = "testTitle2", Url = "testUrl2", Id = 2 },
                new SearchResultModel{ SearchTerm = "testTerm3", Title = "testTitle3", Url = "testUrl3", Id = 3 },
            };

            await repository.AddRange(entities);

            var actualEntities = repository.GetAll();

            actualEntities.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public async Task DeleteTest()
        {
            var repository = GetInMemorySearchResultsRepository();
            var entity = new SearchResultModel { SearchTerm = "testTerm", Title = "testTitle", Url = "testUrl", Id = 1 };
            await repository.Add(entity);

            await repository.Delete(1);

            var actualEntities = repository.GetAll();
            var expectedCountOfEntities = 0;

            Assert.Equal(expectedCountOfEntities, actualEntities.Count());
        }

        [Fact]
        public async Task DeleteRangeTest()
        {
            var repository = GetInMemorySearchResultsRepository();
            var entities = new List<SearchResultModel> {
                new SearchResultModel{ SearchTerm = "testTerm", Title = "testTitle", Url = "testUrl", Id = 1 },
                new SearchResultModel{ SearchTerm = "testTerm2", Title = "testTitle2", Url = "testUrl2", Id = 2 },
                new SearchResultModel{ SearchTerm = "testTerm3", Title = "testTitle3", Url = "testUrl3", Id = 3 },
            };

            await repository.AddRange(entities);
            await repository.DeleteRange(entities);

            var actualEntities = repository.GetAll();
            var expectedCountOfEntities = 0;

            Assert.Equal(expectedCountOfEntities, actualEntities.Count());
        }

        [Fact]
        public async Task GetBySearchTermTest()
        {
            var repository = GetInMemorySearchResultsRepository();
            await repository.Add(new SearchResultModel { SearchTerm = "testTerm", Title = "testTitle", Url = "testUrl", Id = 1 });
            await repository.Add(new SearchResultModel { SearchTerm = "testTerm2", Title = "testTitle2", Url = "testUrl2", Id = 2 });
            await repository.Add(new SearchResultModel { SearchTerm = "testTerm", Title = "testTitle3", Url = "testUrl3", Id = 3 });

            var actualEntities = repository.GetBySearchTerm("testTerm").ToList();
            var expectedEntities = new List<SearchResultModel>
            {
                new SearchResultModel{ SearchTerm = "testTerm", Title = "testTitle", Url = "testUrl", Id = 1 },
                new SearchResultModel{ SearchTerm = "testTerm", Title = "testTitle3", Url = "testUrl3", Id = 3 }
            };

            actualEntities.Should().BeEquivalentTo(expectedEntities);
        }

        private ISearchResultsRepository GetInMemorySearchResultsRepository()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                 .UseInMemoryDatabase(databaseName: "TestDB")
                 .Options;
            var applicationContext = new ApplicationContext(options);
            applicationContext.Database.EnsureDeleted();
            applicationContext.Database.EnsureCreated();
            return new SearchResultsRepository(applicationContext);
        }
    }
}