
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace FlightBooking_CaseStudy_sameer.Models
{


    public class User
    {
        [Key]  
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime DOB { get; set; }

        public int Age { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string Role { get; set; }

        // [JsonIgnore]
        // public int FailedLoginAttempts { get; set; }
        // [JsonIgnore]
        // public DateTime? LastFailedLoginAttempt { get; set; }

        
        public ICollection<Booking> Bookings { get; set; }
    }
}
