using System;

using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
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
        public async void IntegrationTestGetMethod()
        {
            string expected = "{\"totalItem\":2,\"perPage\":5,\"totalPageNumber\":1,\"currentPage\":1,\"FirstRowOnPage\":1,\"LastRowOnPage\":2,\"Profiles\":[{\"id\":1,\"name\":\"iFew\",\"about_us\":\"Hello World!\",\"add_datetime\":\"2019-01-16T11:59:59\"},{\"id\":2,\"name\":\"Chitpong\",\"about_us\":\"My Name is Chitpong\",\"add_datetime\":\"2019-01-16T12:00:00\"}]}";
            
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            Function functions = new Function();

            request = new APIGatewayProxyRequest();
            context = new TestLambdaContext();
            response = await functions.Get(request, context);
            ListProfileModel responseBody = JsonConvert.DeserializeObject<ListProfileModel>(response.Body);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(expected, response.Body);
            Assert.Equal(2, responseBody.totalItem);
        }


        [Fact]
        public async void TestGetMethod()
        {
            string expected = "{\"totalItem\":2,\"perPage\":5,\"totalPageNumber\":1,\"currentPage\":1,\"FirstRowOnPage\":1,\"LastRowOnPage\":2,\"Profiles\":[{\"id\":1,\"name\":\"iFew\",\"about_us\":\"Hello World!\",\"add_datetime\":\"2019-01-16T11:59:59\"},{\"id\":2,\"name\":\"Chitpong\",\"about_us\":\"My Name is Chitpong\",\"add_datetime\":\"2019-01-16T12:00:00\"}]}";
            
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
            response = await functions.Get(request, context);
            
            ListProfileModel responseBody = JsonConvert.DeserializeObject<ListProfileModel>(response.Body);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(expected, response.Body);
            Assert.Equal(2, responseBody.totalItem);
        }

        [Fact]
        public async void TestServiceAsync()
        {
            string expected = "{\"totalItem\":2,\"perPage\":5,\"totalPageNumber\":1,\"currentPage\":1,\"FirstRowOnPage\":1,\"LastRowOnPage\":2,\"Profiles\":[{\"id\":1,\"name\":\"iFew\",\"about_us\":\"Hello World!\",\"add_datetime\":\"2019-01-16T11:59:59\"},{\"id\":2,\"name\":\"Chitpong\",\"about_us\":\"My Name is Chitpong\",\"add_datetime\":\"2019-01-16T12:00:00\"}]}";
            FilterRequestModel querystring = new FilterRequestModel {
                Limit = "5",
                Page = "1"
            };

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

            var response = await service.ListProfileAsync(querystring);
            ListProfileModel responseBody = JsonConvert.DeserializeObject<ListProfileModel>(response.Body);
            
            Assert.Equal(200, response.StatusCode);
            Assert.Equal(expected, response.Body);
            Assert.Equal(2, responseBody.totalItem);
        }

        [Fact]
        public async void When_Have_6Profiles_But_Display_5PerPage_Should_Be_Display_5Profiles()
        {
            string expected = "{\"totalItem\":6,\"perPage\":5,\"totalPageNumber\":2,\"currentPage\":1,\"FirstRowOnPage\":1,\"LastRowOnPage\":5,\"Profiles\":[{\"id\":1,\"name\":\"iFew\",\"about_us\":\"Hello World!\",\"add_datetime\":\"2019-01-16T11:59:59\"},{\"id\":2,\"name\":\"Chitpong\",\"about_us\":\"My Name is Chitpong\",\"add_datetime\":\"2019-01-16T12:00:00\"},{\"id\":3,\"name\":\"Chitpong\",\"about_us\":\"My Name is Chitpong\",\"add_datetime\":\"2019-01-16T12:00:00\"},{\"id\":4,\"name\":\"Chitpong\",\"about_us\":\"My Name is Chitpong\",\"add_datetime\":\"2019-01-16T12:00:00\"},{\"id\":5,\"name\":\"Chitpong\",\"about_us\":\"My Name is Chitpong\",\"add_datetime\":\"2019-01-16T12:00:00\"}]}";
            FilterRequestModel querystring = new FilterRequestModel {
                Limit = "5",
                Page = "1"
            };

            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("list_paging_profile").Options;
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
                
            db_context.Profiles.Add(new ProfileModel { 
                    id = 3,
                    name = "Chitpong",
                    about_us = "My Name is Chitpong",
                    add_datetime = DateTime.Parse("2019-01-16 12:00:00")
                });
                
            db_context.Profiles.Add(new ProfileModel { 
                    id = 4,
                    name = "Chitpong",
                    about_us = "My Name is Chitpong",
                    add_datetime = DateTime.Parse("2019-01-16 12:00:00")
                });
                
            db_context.Profiles.Add(new ProfileModel { 
                    id = 5,
                    name = "Chitpong",
                    about_us = "My Name is Chitpong",
                    add_datetime = DateTime.Parse("2019-01-16 12:00:00")
                });
                
            db_context.Profiles.Add(new ProfileModel { 
                    id = 6,
                    name = "Chitpong",
                    about_us = "My Name is Chitpong",
                    add_datetime = DateTime.Parse("2019-01-16 12:00:00")
                });
            db_context.SaveChanges();

            ProfileService service = new ProfileService(db_context);

            var response = await service.ListProfileAsync(querystring);
            ListProfileModel responseBody = JsonConvert.DeserializeObject<ListProfileModel>(response.Body);
            
            Assert.Equal(200, response.StatusCode);
            Assert.Equal(expected, response.Body);
            Assert.Equal(6, responseBody.totalItem);
            Assert.Equal(5, responseBody.perPage);
            Assert.Equal(1, responseBody.currentPage);
            Assert.Equal(2, responseBody.totalPageNumber);
        }
    }
}
