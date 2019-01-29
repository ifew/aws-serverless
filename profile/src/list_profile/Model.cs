using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Amazon.Lambda.APIGatewayEvents;

namespace list_profile
{
    [Table("profiles")]
    public class ProfileModel {
        public int id { get; set; }
        public string name { get; set; }
        public string about_us { get; set; }
        public DateTime add_datetime { get; set; }
    }

    public class PagingAPIGatewayProxyResponse : APIGatewayProxyResponse {
        public Paging Paging { get; set; }
    }

    public class Paging
    {
        public int totalItem { get; set; }
        public int perPage { get; set; }
        public int totalPageNumber { get; set; }
        public int currentPage { get; set; } 

        public int FirstRowOnPage
        {
            get { return (currentPage - 1) * perPage + 1; }
        }
        public int LastRowOnPage
        {
            get { return Math.Min(currentPage * perPage, totalItem); }
        }
    }
}