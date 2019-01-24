using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using update_profile;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Amazon.Lambda.Serialization.Json;
using System.IO;

namespace update_profile.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        //[Fact]
        public void IntegrationTestGetMethod()
        {
            var requestString = File.ReadAllText("./SampleRequests/TestPutMethod.json");

            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            Function functions = new Function();
            
            request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestString);
            context = new TestLambdaContext();
            response = functions.Put(request, context);

            var response_data = JsonConvert.DeserializeObject<ProfileModel>(response.Body);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("1", response_data.id.ToString());
            Assert.Equal("yourFew", response_data.name);
            Assert.Equal("Hello Mars!", response_data.about_us);
        }

        // [Fact]
        // public void TestGetMethod()
        // {
        //     var requestString = File.ReadAllText("./SampleRequests/TestPutMethod.json");

        //     TestLambdaContext context;
        //     APIGatewayProxyRequest request;
        //     APIGatewayProxyResponse response;

        //     var provider = new ServiceCollection()
        //     .AddDbContext<FunctionContext>(options => options.UseInMemoryDatabase("profile"))
        //     .AddSingleton<ProfileService, ProfileService>()
        //     .BuildServiceProvider();

        //     FunctionContext db_add_context = provider.GetRequiredService<FunctionContext>();
        //         db_add_context.Profiles.Add(new ProfileModel { 
        //             id = 1,
        //             name = "iFew",
        //             about_us = "Hello World!",
        //             add_datetime = DateTime.Parse("2019-01-16 11:59:59")
        //         });
        //         db_add_context.SaveChanges();

        //     Function functions = new Function(provider);

        //     request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestString);
        //     context = new TestLambdaContext();
        //     response = functions.Put(request, context);

        //     var response_data = JsonConvert.DeserializeObject<ProfileModel>(response.Body);

        //     Assert.Equal(200, response.StatusCode);
        //     Assert.Equal("999", response_data.id.ToString());
        //     Assert.Equal("iFew", response_data.name);
        //     Assert.Equal("Hello World!", response_data.about_us);
        // }

        [Fact]
        public void TestService()
        {
            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("update_profile").Options;
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

            var new_data = new ProfileModel { 
                id = 1,
                name = "yourFew",
                about_us = "Hello Mars!",
                add_datetime = DateTime.Parse("2019-01-23 11:59:59")
            };

            ProfileService service = new ProfileService(db_context);

            var response = service.UpdateProfile(new_data);

            var response_data = JsonConvert.DeserializeObject<ProfileModel>(response.Body);
            
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("1", response_data.id.ToString());
            Assert.Equal("yourFew", response_data.name);
            Assert.Equal("Hello Mars!", response_data.about_us);
        }
    }
}
