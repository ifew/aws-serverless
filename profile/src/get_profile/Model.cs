using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace get_profile
{
    [Table("profiles")]
    public class ProfileModel {
        public int id { get; set; }
        public string lang { get; set; }
        public string name { get; set; }
        public string about_us { get; set; }
        public DateTime add_datetime { get; set; }
    }
}