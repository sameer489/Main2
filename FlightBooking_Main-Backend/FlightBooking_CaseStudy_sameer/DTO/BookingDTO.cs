using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace FlightBooking_CaseStudy_sameer.DTOs
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ScheduleId { get; set; }

       

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Tickets can be from 1 to 4.")]
        public int No_of_Sets { get; set; }
         [Required]
        [DataType(DataType.DateTime)]
         public DateTime BookingDate { get; set; }
        // [Required]
        // [Range(1, 999, ErrorMessage = "Seat number must be between 1 and 999.")]
        // public int SeatNumber { get; set; }
          public bool CheckInStatus { get; set; }

        public double TotalAmount { get; set; }
       // public ICollection<FlightDTO> Flight { get; set; }
       // public ICollection<ScheduleDTO> Schedule { get; set; }
         [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
       // [JsonIgnore]
        //public string PaymentMethodId { get; set; }
    }
}
