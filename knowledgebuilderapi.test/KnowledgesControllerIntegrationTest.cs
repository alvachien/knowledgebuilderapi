using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Microsoft.Data.Sqlite;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using knowledgebuilderapi.Models;
using knowledgebuilderapi.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace knowledgebuilderapi.test
{
    public class KnowledgeControllerIntegrationTest :
        IClassFixture<CustomWebApplicationFactory<knowledgebuilderapi.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<knowledgebuilderapi.Startup> _factory;

        public KnowledgeControllerIntegrationTest(
            CustomWebApplicationFactory<knowledgebuilderapi.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Fact]
        public async Task Knowlege_Create_Update_Delete_Test()
        {
            // Metadata request
            var metadata = await _client.GetAsync("/odata/$metadata");
            Assert.Equal(HttpStatusCode.OK, metadata.StatusCode);
            var content = await metadata.Content.ReadAsStringAsync();
            if (content.Length > 0) 
            {
            }

            // Get all
            var req1 = await _client.GetAsync("/odata/Knowledges");
            Assert.Equal(HttpStatusCode.OK, req1.StatusCode);
            content = await req1.Content.ReadAsStringAsync();
            if (content.Length > 0) 
            {
                var resultJson = JObject.Parse(content);
                var value = resultJson.SelectToken("value");
                Assert.Equal(0, value.Count());
            }

            // Get all with count
            var req2 = await _client.GetAsync("/odata/Knowledges?$count=true");
            Assert.Equal(HttpStatusCode.OK, req2.StatusCode);
            content = await req2.Content.ReadAsStringAsync();
            if (content.Length > 0) 
            {
                var resultJson = JObject.Parse(content);
                var cnt = resultJson.GetValue("@odata.count");
                Assert.Equal(0, cnt.Value<int>());                
                var value = resultJson.SelectToken("value");
                Assert.Equal(0, value.Count());
            }

            // Then, create one knowledge
            var nmod = new Knowledge() {
                Title = "Test 1",
                Category = KnowledgeCategory.Concept,
                Content = "My test 1"
            };
            
            var kjson = JsonConvert.SerializeObject(nmod);
            HttpContent inputContent = new StringContent(kjson, Encoding.UTF8, "application/json");
            var req3 = await _client.PostAsync("/odata/Knowledges", inputContent);
            Assert.Equal(HttpStatusCode.OK, req3.StatusCode);
            content = await req3.Content.ReadAsStringAsync();
            if (content.Length > 0) 
            {
                var resultJson = JObject.Parse(content);
                if (resultJson != null) 
                {
                }
            }

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
