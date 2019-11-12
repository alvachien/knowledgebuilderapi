using System;
using Xunit;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using knowledgebuilderapi.Models;
using knowledgebuilderapi.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData.Results;

namespace knowledgebuilderapi.test
{
    public class KnowledgeControllerIntegrationTest :
        IClassFixture<CustomWebApplicationFactory<knowledgebuilderapi.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<knowledgebuilderapi.Startup>
            _factory;

        public KnowledgeControllerIntegrationTest(
            CustomWebApplicationFactory<knowledgebuilderapi.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Post_DeleteAllMessagesHandler_ReturnsRedirectToRoot()
        {
            // // Arrange
            // var defaultPage = await _client.GetAsync("/");
            // var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            // //Act
            // var response = await _client.SendAsync(
            //     (IHtmlFormElement)content.QuerySelector("form[id='messages']"),
            //     (IHtmlButtonElement)content.QuerySelector("button[id='deleteAllBtn']"));

            // // Assert
            // Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            // Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            // Assert.Equal("/", response.Headers.Location.OriginalString);
        }
    }
}
