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
        public async Task<PagingAPIGatewayProxyResponse> Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            System.Console.WriteLine(request);
            var profileService = _service.GetService<ProfileService>();
            var response = await profileService.ListProfileAsync();

            return response;
        }
    }
}
