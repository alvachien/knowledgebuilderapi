﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace knowledgebuilderapi.test.integrationtest
{
    [Collection("KBAPI_IntegrationTests#1")]
    public class ODataEndpointControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ODataEndpointControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory
                //.WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot("test//GalleryAPI.integratintest"))
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }

        [Fact]
        public async Task GetResultPage()
        {
            // Arrange
            var defaultPage = await _client.GetAsync("/$odata");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            Assert.NotNull(content);
            Assert.Equal("http://localhost/$odata", content.Url);
        }
    }
}
