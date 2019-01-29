using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace get_profile
{
    public class ProfileService
    {

        private readonly FunctionContext _context_db;
        public ProfileService(FunctionContext context_db) {
            _context_db = context_db;
        }

        public APIGatewayProxyResponse GetProfile(string id)
        {
            ProfileModel data = _context_db.Profiles.Where(p => p.id == Int32.Parse(id)).SingleOrDefault();

            APIGatewayProxyResponse respond = new APIGatewayProxyResponse {
                StatusCode = (int)HttpStatusCode.OK,
                Headers = new Dictionary<string, string>
                { 
                    { "Content-Type", "application/json" }, 
                    { "Access-Control-Allow-Origin", "*" } 
                },
                Body = JsonConvert.SerializeObject(data)
            };

            return respond;
        }
    }
}
