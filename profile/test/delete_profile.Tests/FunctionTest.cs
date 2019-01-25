using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using delete_profile;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Amazon.Lambda.Serialization.Json;
using System.IO;

namespace delete_profile.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        //[Fact]
        public void IntegrationTestGetMethod()
        {
            var requestString = File.ReadAllText("./SampleRequests/TestDeleteMethod.json");

            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            Function functions = new Function();
            
            request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestString);
            context = new TestLambdaContext();
            response = functions.Delete(request, context);

            var response_data = JsonConvert.DeserializeObject<RespondModel>(response.Body);
            
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("true", response_data.result.ToString());
        }

        [Fact]
        public void TestGetMethod()
        {
            var requestString = File.ReadAllText("./SampleRequests/TestDeleteMethod.json");

            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            var provider = new ServiceCollection()
            .AddDbContext<FunctionContext>(options => options.UseInMemoryDatabase("delete_profile"))
            .AddSingleton<ProfileService, ProfileService>()
            .BuildServiceProvider();

            FunctionContext db_add_context = provider.GetRequiredService<FunctionContext>();
            db_add_context.Profiles.Add(new ProfileModel { 
                id = 1,
                name = "iFew",
                about_us = "Hello World!",
                add_datetime = DateTime.Parse("2019-01-16 11:59:59")
            });
            db_add_context.SaveChanges();

            Function functions = new Function(provider);

            request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestString);
            context = new TestLambdaContext();
            response = functions.Delete(request, context);

            var response_data = JsonConvert.DeserializeObject<RespondModel>(response.Body);
            
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("true", response_data.result.ToString());
        }

        [Fact]
        public void TestService()
        {
            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("delete_profile").Options;
            FunctionContext db_context = new FunctionContext(_options);

            using (FunctionContext db_add_context = new FunctionContext(_options))
            {
                db_add_context.Profiles.Add(new ProfileModel { 
                    id = 1,
                    name = "iFew",
                    about_us = "Hello World!",
                    add_datetime = DateTime.Parse("2019-01-16 11:59:59")
                });
                db_add_context.SaveChanges();
            }

            ProfileService service = new ProfileService(db_context);

            var response = service.DeleteProfile("1");

            var response_data = JsonConvert.DeserializeObject<RespondModel>(response.Body);
            
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("true", response_data.result.ToString());
        }
    }
}
