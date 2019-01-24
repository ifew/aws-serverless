using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace add_profile
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
        /// Add new profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of profile</returns>
        public APIGatewayProxyResponse Post(ProfileModel profileInput, ILambdaContext context)
        {
            if (!String.IsNullOrEmpty(profileInput.name))
            {

                var profileService = _service.GetService<ProfileService>();
                var response = profileService.AddProfile(profileInput);

                return response;
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }
}
