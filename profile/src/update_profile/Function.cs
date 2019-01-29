using System;
using System.Net;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace update_profile
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
        /// Update data profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of profile</returns>
        public APIGatewayProxyResponse Put(APIGatewayProxyRequest request, ILambdaContext context)
        {
            ProfileModel profileInput = JsonConvert.DeserializeObject<ProfileModel>(request.Body);
            if (!String.IsNullOrEmpty(profileInput.name))
            {

                var profileService = _service.GetService<ProfileService>();
                var response = profileService.UpdateProfile(profileInput);

                return response;
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }
}
