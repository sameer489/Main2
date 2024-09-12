using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking_CaseStudy_sameer.Models
{
   

    public class Booking
    {
        [Key]  // Primary Key
        public int BookingId { get; set; }

        [ForeignKey("User")]  // Foreign Key referencing User
        public int UserId { get; set; }

        [ForeignKey("Schedule")]  // Foreign Key referencing Schedule
        public int ScheduleId { get; set; }

     
        public string Name { get; set; }

        public DateTime BookingDate { get; set; }

        public int No_of_Sets { get; set; }

        //public int SeatNumber { get; set; }

          public bool CheckInStatus { get; set; }=false;

        public double TotalAmount { get; set; }

        // public int No_of_Sets { get; internal set; }

        public string Email { get; set; }

         public ICollection<User> User { get; set; }

        public ICollection<Schedule> Schedule { get; set; }

        
    }

}
