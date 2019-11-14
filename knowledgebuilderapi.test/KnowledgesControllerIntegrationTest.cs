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
using Newtonsoft.Json.Converters;
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
            List<Int32> listCreatedIds = new List<Int32>();

            // Step 1. Metadata request
            var metadata = await _client.GetAsync("/odata/$metadata");
            Assert.Equal(HttpStatusCode.OK, metadata.StatusCode);
            var content = await metadata.Content.ReadAsStringAsync();
            if (content.Length > 0) 
            {
                // How to verify metadata?
                // TBD.
            }

            // Step 2. Get all knowledges - empty
            var req1 = await _client.GetAsync("/odata/Knowledges");
            Assert.Equal(HttpStatusCode.OK, req1.StatusCode);
            content = await req1.Content.ReadAsStringAsync();
            if (content.Length > 0) 
            {
                JToken outer = JToken.Parse(content);

                JArray inner = outer["value"].Value<JArray>();
                Assert.Empty(inner);
            }

            // Step 3. Get all knowledge with count - zero and empty
            var req2 = await _client.GetAsync("/odata/Knowledges?$count=true");
            Assert.Equal(HttpStatusCode.OK, req2.StatusCode);
            content = await req2.Content.ReadAsStringAsync();
            if (content.Length > 0) 
            {
                JToken outer = JToken.Parse(content);

                Int32 odatacount = outer["@odata.count"].Value<Int32>();
                Assert.Equal(0, odatacount);

                JArray inner = outer["value"].Value<JArray>();
                Assert.Empty(inner);
            }

            // Step 4. Create first knowledge
            var nmod = new Knowledge() 
            {
                Title = "Test 1",
                Category = KnowledgeCategory.Concept,
                Content = "My test 1"
            };
            
            var jsetting = new JsonSerializerSettings();
            jsetting.Converters.Add(new StringEnumConverter());
            var kjson = JsonConvert.SerializeObject(nmod, jsetting);
            HttpContent inputContent = new StringContent(kjson, Encoding.UTF8, "application/json");
            var req3 = await _client.PostAsync("/odata/Knowledges", inputContent);
            Assert.Equal(HttpStatusCode.Created, req3.StatusCode);
            content = await req3.Content.ReadAsStringAsync();
            if (content.Length > 0) 
            {
                var nmod2 = JsonConvert.DeserializeObject<Knowledge>(content);
                Assert.Equal(nmod.Title, nmod2.Title);
                Assert.Equal(nmod.Content, nmod2.Content);
                listCreatedIds.Add(nmod2.ID);
            }

            // Step 5. Get all knowledge with count - one and an array with single item
            req2 = await _client.GetAsync("/odata/Knowledges?$count=true");
            Assert.Equal(HttpStatusCode.OK, req2.StatusCode);
            content = await req2.Content.ReadAsStringAsync();
            if (content.Length > 0) 
            {
                JToken outer = JToken.Parse(content);

                Int32 odatacount = outer["@odata.count"].Value<Int32>();
                Assert.Equal(1, odatacount);

                JArray inner = outer["value"].Value<JArray>();
                Assert.Single(inner);
                
                foreach (var id in inner) 
                {
                    JObject dv = id.Value<JObject>();
                    foreach(var prop in dv.Properties().Select(p => p.Name).ToList())
                    {
                        switch(prop)
                        {
                            case "ID":
                                int nid = dv[prop].Value<Int32>();
                                Assert.Equal(listCreatedIds[0], nid);
                                break;

                            case "Title":
                                string dv_t = dv[prop].Value<string>();
                                Assert.Equal(nmod.Title, dv_t);
                            break;
                            
                            case "Content":
                                string dv_c = dv[prop].Value<string>();
                                Assert.Equal(nmod.Content, dv_c);
                            break;

                            default:
                            break;
                        }
                    }
                }
            }

            // Step 6: Create second knowledge
            nmod = new Knowledge() 
            {
                Title = "Test 2",
                Category = KnowledgeCategory.Formula,
                Content = "My test 2"
            };
            
            kjson = JsonConvert.SerializeObject(nmod, jsetting);
            inputContent = new StringContent(kjson, Encoding.UTF8, "application/json");
            var req6 = await _client.PostAsync("/odata/Knowledges", inputContent);
            Assert.Equal(HttpStatusCode.Created, req6.StatusCode);
            content = await req6.Content.ReadAsStringAsync();
            if (content.Length > 0) 
            {
                var nmod2 = JsonConvert.DeserializeObject<Knowledge>(content);
                Assert.Equal(nmod.Title, nmod2.Title);
                Assert.Equal(nmod.Content, nmod2.Content);
                listCreatedIds.Add(nmod2.ID);
            }
        }
    }
}
