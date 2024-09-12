using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace FlightBooking_CaseStudy_sameer.Models
{
    

    public class Admin
    {
        [Key]  // Primary Key
        public int AdminId { get; set; }
        public string AdminName { get; set; }

        public string AdminEmail { get; set; }

        public string AdminPassword { get; set; }
        // [JsonIgnore]
        // public int FailedLoginAttempts { get; set; }
        // [JsonIgnore]
        // public DateTime? LastFailedLoginAttempt { get; set; } 
        public string Role { get; set; }
       
    }

}
