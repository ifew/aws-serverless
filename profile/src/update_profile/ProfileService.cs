using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
              var current_data=  _context_db.Profiles.FirstOrDefault(p => p.id == data.id);
            _context_db.Entry(current_data).CurrentValues.SetValues(data);
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
