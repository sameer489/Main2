using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking_CaseStudy_sameer.Models
{
    

    public class Schedule
    {
        [Key]
        public int schedule_id { get; set; }
 
        public string Flight_Name{get; set;}
 
        public int Seat_Capacity{get; set;}
 
        public string Start_location { get; set; }
 
        public string Destination { get; set; }
 
        public DateOnly Travel_date { get; set; }
 
        public TimeSpan Arrival_Time { get; set; }
        public TimeSpan Depature_Time { get; set; }
 
        public double Fare { get;set; }
 
        public ICollection<Booking> Bookings { get; set; }
 
    }

}

