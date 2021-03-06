using System;
using System.Net;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.Extensions.DependencyInjection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace get_profile
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
        /// Get profile by id
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of profile</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            string profileID = null;
            string langID = null;
            if (request.PathParameters != null && request.PathParameters.ContainsKey("id"))
                profileID = request.PathParameters["id"];

            
            if (request.PathParameters != null && request.PathParameters.ContainsKey("lang"))
                langID = request.PathParameters["lang"];

            if (!String.IsNullOrEmpty(profileID))
            {

                var profileService = _service.GetService<ProfileService>();
                var response = profileService.GetProfile(profileID, langID);

                return response;
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }
    }
}
