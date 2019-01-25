using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Microsoft.EntityFrameworkCore;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace delete_profile
{
    public class ProfileService
    {

        private readonly FunctionContext _context_db;
        public ProfileService(FunctionContext context_db) {
            _context_db = context_db;
        }

        public APIGatewayProxyResponse DeleteProfile(string profile_id)
        {
            var profile = new ProfileModel { id = Int32.Parse(profile_id) };
            _context_db.Profiles.Remove(profile);
            var result = _context_db.SaveChanges();

            APIGatewayProxyResponse respond = new APIGatewayProxyResponse {
                StatusCode = (int)HttpStatusCode.OK,
                Headers = new Dictionary<string, string>
                { 
                    { "Content-Type", "application/json" }, 
                    { "Access-Control-Allow-Origin", "*" } 
                },
                Body = JsonConvert.SerializeObject(result)
            };

            return respond;
        }
    }
}
