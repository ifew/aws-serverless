using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Microsoft.EntityFrameworkCore;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace list_profile
{
    public class ProfileService
    {

        private readonly FunctionContext _context_db;
        public ProfileService(FunctionContext context_db) {
            _context_db = context_db;
        }

        public async Task<PagingAPIGatewayProxyResponse> ListProfileAsync()
        {
            decimal perPage = 5;
            int currentPage = 1;
            int skip = (currentPage - 1) * (int)perPage;

            List<ProfileModel> data = await _context_db.Profiles.Skip(skip).Take((int)perPage).ToListAsync();
            decimal totalData = await _context_db.Profiles.CountAsync();
            int totalPageNumber = (int)Math.Ceiling(totalData / perPage);
 

            PagingAPIGatewayProxyResponse respond = new PagingAPIGatewayProxyResponse {
                StatusCode = (int)HttpStatusCode.OK,
                Headers = new Dictionary<string, string>
                { 
                    { "Content-Type", "application/json" }, 
                    { "Access-Control-Allow-Origin", "*" } 
                },
                Body = JsonConvert.SerializeObject(data),
                Paging = new Paging {
                    totalItem = (int)totalData,
                    perPage = (int)perPage,
                    totalPageNumber = totalPageNumber,
                    currentPage = currentPage
                }
            };

            return respond;
        }
    }
}
