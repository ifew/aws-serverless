using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
            _context_db.Remove(_context_db.Profiles.Single(a => a.id == Int32.Parse(profile_id)));
            var save_result = _context_db.SaveChanges();

            var result = new RespondModel {
                result = "true"
            };
            if(save_result < 1) {
                result.result = "false";
            }

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
