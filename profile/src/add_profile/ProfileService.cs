using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace add_profile
{
    public class ProfileService
    {

        private readonly FunctionContext _context_db;
        public ProfileService(FunctionContext context_db) {
            _context_db = context_db;
        }

        public APIGatewayProxyResponse AddProfile(ProfileModel data)
        {
            data.add_datetime = DateTime.Now;
            
            _context_db.Profiles.Add(data);
            _context_db.SaveChanges();

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
