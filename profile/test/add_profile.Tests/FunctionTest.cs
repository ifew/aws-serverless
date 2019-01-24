using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using add_profile;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Amazon.Lambda.Serialization.Json;
using System.IO;

namespace add_profile.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        //[Fact]
        // public void IntegrationTestGetMethod()
        // {
        //     string expected = "{\"id\":1,\"name\":\"iFew\",\"about_us\":\"Hello World!\",\"add_datetime\":\"2019-01-16T11:59:59\"}";
        //     var requestString = File.ReadAllText("./SampleRequests/TestGetMethod.json");

        //     TestLambdaContext context;
        //     APIGatewayProxyRequest request;
        //     APIGatewayProxyResponse response;

        //     Function functions = new Function();
            
        //     request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestString);
        //     context = new TestLambdaContext();
        //     response = functions.Get(request, context);

        //     Assert.Equal(200, response.StatusCode);
        //     Assert.Equal(expected, response.Body);
        // }

        [Fact]
        public void TestGetMethod()
        {
            var requestString = File.ReadAllText("./SampleRequests/TestPostMethod.json");

            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            var provider = new ServiceCollection()
            .AddDbContext<FunctionContext>(options => options.UseInMemoryDatabase("profile"))
            .AddSingleton<ProfileService, ProfileService>()
            .BuildServiceProvider();

            Function functions = new Function(provider);

            request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestString);
            context = new TestLambdaContext();
            response = functions.Post(request, context);

            var response_data = JsonConvert.DeserializeObject<ProfileModel>(response.Body);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("1", response_data.id.ToString());
            Assert.Equal("iFew", response_data.name);
            Assert.Equal("Hello World!", response_data.about_us);
        }

        [Fact]
        public void TestService()
        {
            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("add_profile").Options;
            FunctionContext db_context = new FunctionContext(_options);

            ProfileService service = new ProfileService(db_context);

            var data = new ProfileModel { 
                id = 1,
                name = "iFew",
                about_us = "Hello World!"
            };

            var response = service.AddProfile(data);

            var response_data = JsonConvert.DeserializeObject<ProfileModel>(response.Body);
            
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("1", response_data.id.ToString());
            Assert.Equal("iFew", response_data.name);
            Assert.Equal("Hello World!", response_data.about_us);
        }
    }
}
