using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.Extensions.DependencyInjection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace list_profile
{
    public class Function
    {
        private ServiceProvider _service;

        public Function()
            : this (Bootstrap.CreateInstance()) {}

        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Function(ServiceProvider service)
        {
            _service = service;
        }

        /// <summary>
        /// List profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of profile</returns>
        public async Task<APIGatewayProxyResponse> Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            string page = "1";
            string limit = "5";
            
            if(request.QueryStringParameters != null && request.QueryStringParameters.ContainsKey("pageNumber")) {
                page = request.QueryStringParameters["pageNumber"];
            }

            if(request.QueryStringParameters != null && request.QueryStringParameters.ContainsKey("pageSize") == false) {
                limit = request.QueryStringParameters["pageSize"];
            }

            FilterRequestModel filter = new FilterRequestModel {
                Page = page,
                Limit = limit
            };
            
            var profileService = _service.GetService<ProfileService>();
            var response = await profileService.ListProfileAsync(filter);

            return response;
        }
    }
}
