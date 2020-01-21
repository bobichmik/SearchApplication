using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Searching;
using Moq;
using SearchApp.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RazorPagesTests
{
    public class RazorPagesTests
    {
        [Fact]
        public async Task OnPostIndexPageTest()
        {
            var repository = new Mock<ISearchResultsRepository>();
            var client = new Mock<ISearchClient>();
            var entities = new List<SearchResultModel> {
                new SearchResultModel{ SearchTerm = "testTerm", Title = "testTitle", Url = "testUrl", Id = 1 },
                new SearchResultModel{ SearchTerm = "testTerm", Title = "testTitle2", Url = "testUrl2", Id = 2 },
                new SearchResultModel{ SearchTerm = "testTerm", Title = "testTitle3", Url = "testUrl3", Id = 3 },
            };

            client.Setup(x => x.GetSearchInfoAsync("testTerm")).Returns(Task.FromResult(entities));
            repository.Setup(x => x.AddRange(entities)).Returns(Task.CompletedTask);

            var pageModel = new IndexModel(new List<ISearchClient> { client.Object }, repository.Object);

            await pageModel.OnPostAsync("testTerm");

            Assert.Equal(entities, pageModel.DisplayedResults);
        }

        [Fact]
        public void OnPostDbSearchPageTest()
        {
            var repository = new Mock<ISearchResultsRepository>();
            var entities = new List<SearchResultModel> {
                new SearchResultModel{ SearchTerm = "testTerm", Title = "testTitle", Url = "testUrl", Id = 1 },
                new SearchResultModel{ SearchTerm = "testTerm", Title = "testTitle2", Url = "testUrl2", Id = 2 },
                new SearchResultModel{ SearchTerm = "testTerm", Title = "testTitle3", Url = "testUrl3", Id = 3 },
            };

            repository.Setup(x => x.GetBySearchTerm("testTerm")).Returns(entities);
            var dbSearchPageModel = new DbSearchModel(repository.Object);

            dbSearchPageModel.OnPostAsync("testTerm");
            Assert.Equal(entities, dbSearchPageModel.DisplayedResults);
        }
    }
}