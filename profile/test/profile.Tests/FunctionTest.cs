using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using list_profile;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace list_profile.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        //[Fact]
        public void IntegrationTestGetMethod()
        {
            string expected = "[{\"id\":1,\"name\":\"iFew\",\"about_us\":\"Hello World!\",\"add_datetime\":\"2019-01-16T11:59:59\"},{\"id\":2,\"name\":\"Chitpong\",\"about_us\":\"My Name is Chitpong\",\"add_datetime\":\"2019-01-16T12:00:00\"}]";
            
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            Function functions = new Function();

            request = new APIGatewayProxyRequest();
            context = new TestLambdaContext();
            response = functions.Get(request, context);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(expected, response.Body);
        }

        [Fact]
        public void TestGetMethod()
        {
            string expected = "[{\"id\":1,\"name\":\"iFew\",\"about_us\":\"Hello World!\",\"add_datetime\":\"2019-01-16T11:59:59\"},{\"id\":2,\"name\":\"Chitpong\",\"about_us\":\"My Name is Chitpong\",\"add_datetime\":\"2019-01-16T12:00:00\"}]";
            
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            var provider = new ServiceCollection()
            .AddDbContext<FunctionContext>(options => options.UseInMemoryDatabase("profile"))
            .AddSingleton<ProfileService, ProfileService>()
            .BuildServiceProvider();

            FunctionContext db_context = provider.GetRequiredService<FunctionContext>();
            db_context.Profiles.Add(new ProfileModel { 
                    id = 1,
                    name = "iFew",
                    about_us = "Hello World!",
                    add_datetime = DateTime.Parse("2019-01-16 11:59:59")
                });
                
            db_context.Profiles.Add(new ProfileModel { 
                    id = 2,
                    name = "Chitpong",
                    about_us = "My Name is Chitpong",
                    add_datetime = DateTime.Parse("2019-01-16 12:00:00")
                });
            db_context.SaveChanges();

            Function functions = new Function(provider);

            request = new APIGatewayProxyRequest();
            context = new TestLambdaContext();
            response = functions.Get(request, context);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(expected, response.Body);
        }

        [Fact]
        public void TestService()
        {
            string expected = "[{\"id\":1,\"name\":\"iFew\",\"about_us\":\"Hello World!\",\"add_datetime\":\"2019-01-16T11:59:59\"},{\"id\":2,\"name\":\"Chitpong\",\"about_us\":\"My Name is Chitpong\",\"add_datetime\":\"2019-01-16T12:00:00\"}]";
            
            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("list_profile").Options;
            FunctionContext db_context = new FunctionContext(_options);

            db_context.Profiles.Add(new ProfileModel { 
                    id = 1,
                    name = "iFew",
                    about_us = "Hello World!",
                    add_datetime = DateTime.Parse("2019-01-16 11:59:59")
                });
                
            db_context.Profiles.Add(new ProfileModel { 
                    id = 2,
                    name = "Chitpong",
                    about_us = "My Name is Chitpong",
                    add_datetime = DateTime.Parse("2019-01-16 12:00:00")
                });
            db_context.SaveChanges();

            ProfileService service = new ProfileService(db_context);

            var response = service.ListProfile();
            
            Assert.Equal(200, response.StatusCode);
            Assert.Equal(expected, response.Body);
        }
    }
}
