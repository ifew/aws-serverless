using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace list_profile
{
    [Table("profiles")]
    public class ProfileModel {
        public int id { get; set; }
        public string name { get; set; }
        public string about_us { get; set; }
        public DateTime add_datetime { get; set; }
    }

    public class FilterRequestModel
    {
        public string Page { get; set; }
        public string Limit { get; set; }
        public string Sort { get; set; }
        public string SortBy { get; set; }
        public string Keyword { get; set; }
    }

    public class ListProfileModel
    {
        public string version { get; set; }
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

        public List<ProfileModel> Profiles { get; set; } 
    }
}