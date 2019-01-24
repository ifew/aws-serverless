using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Microsoft.EntityFrameworkCore;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace update_profile
{
    public class ProfileService
    {

        private readonly FunctionContext _context_db;
        public ProfileService(FunctionContext context_db) {
            _context_db = context_db;
        }

        public APIGatewayProxyResponse UpdateProfile(ProfileModel data)
        {
            data.add_datetime = DateTime.Now;
            _context_db.Entry(data).State = EntityState.Modified;
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
