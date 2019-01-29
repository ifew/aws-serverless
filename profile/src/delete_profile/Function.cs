using System;
using System.Net;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace delete_profile
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
        /// Delete profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns>true/false</returns>
        public APIGatewayProxyResponse Delete(APIGatewayProxyRequest request, ILambdaContext context)
        {
            ProfileModel profileInput = JsonConvert.DeserializeObject<ProfileModel>(request.Body);
            string id = profileInput.id.ToString();
            if (!String.IsNullOrEmpty(id))
            {

                var profileService = _service.GetService<ProfileService>();
                var response = profileService.DeleteProfile(id);

                return response;
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }
}
