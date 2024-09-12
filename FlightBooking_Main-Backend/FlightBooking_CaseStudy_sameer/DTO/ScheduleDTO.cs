
using System;
using System.ComponentModel.DataAnnotations;
// using FlightBooking_CaseStudy_sameer.Converters;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace FlightBooking_CaseStudy_sameer.DTOs
{
    // public class ScheduleDTO : IValidatableObject
    // {
    //     public int ScheduleId { get; set; }

    //     [Required]
    //     public int FlightId { get; set; }

    //     [Required]
    //     [StringLength(100, ErrorMessage = "StartLocation cannot be longer than 100 characters.")]
    //     public string StartLocation { get; set; }

    //     [Required]
    //     [StringLength(100, ErrorMessage = "Destination cannot be longer than 100 characters.")]
    //     public string Destination { get; set; }

    //     [Required]
    //     [DataType(DataType.Date)]
    //     public DateTime TravelDate { get; set; }

    //     [Required]
    //     [Range(0.01, double.MaxValue, ErrorMessage = "Fare must be greater than zero.")]
    //     public double Fare { get; set; }
    
    //     public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //     {
    //         if (StartLocation.Equals(Destination, StringComparison.OrdinalIgnoreCase))
    //         {
    //             yield return new ValidationResult("Source and Destination should not be the same.", new[] { nameof(StartLocation), nameof(Destination) });
    //         }

    //         if (TravelDate.Date < DateTime.Now.Date)
    //         {
    //             yield return new ValidationResult("Travel date should not be in the past.", new[] { nameof(TravelDate) });
    //         }
    //     }
    // }
//     public class ScheduleDTO : IValidatableObject
//    {
//     public int ScheduleId { get; set; }

//     [Required]
//     // [StringLength(100, ErrorMessage = "Flight Name cannot be longer than 100 characters.")]
//     public string FlightName { get; set; }

//     [Required]
//     // [Range(0, int.MaxValue, ErrorMessage = "Seat Capacity must be a positive number.")]
//     public int SeatCapacity { get; set; }

//     [Required]
//     // [StringLength(100, ErrorMessage = "Start Location cannot be longer than 100 characters.")]
//     public string StartLocation { get; set; }

//     [Required]
//     // [StringLength(3, ErrorMessage = "Destination cannot be longer than 100 characters.")]
//     //  [Required]
//     public string Destination { get; set; }
//         [Required]
//         [JsonConverter(typeof(DateOnlyJsonConverter))]
//         public DateOnly TravelDate { get; set; } 

//         // Use TimeOnly for times
//          [Required]
//         [JsonConverter(typeof(TimeOnlyJsonConverter))]

//         public TimeOnly ArrivalTime { get; set; }

//         [Required]

//         [JsonConverter(typeof(TimeOnlyJsonConverter))]
//         public TimeOnly DepartureTime { get; set; }

//     [Required]
//     // [Range(0, double.MaxValue, ErrorMessage = "Fare must be greater than zero.")]
//     public double Fare { get; set; }

//     public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
//     {
//         if (StartLocation.Equals(Destination, StringComparison.OrdinalIgnoreCase))
//         {
//             yield return new ValidationResult("Start Location and Destination should not be the same.", new[] { nameof(StartLocation), nameof(Destination) });
//         }

//         if (TravelDate.Date < DateTime.Now.Date)
//         {
//             yield return new ValidationResult("Travel Date should not be in the past.", new[] { nameof(TravelDate) });
//         }

//         // if (ArrivalTime <= DepartureTime)
//         // {
//         //     yield return new ValidationResult("Arrival Time should be after Departure Time.", new[] { nameof(ArrivalTime), nameof(DepartureTime) });
//         // }
//     }
//   }
    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ScheduleDTO : IValidatableObject
{
    public int ScheduleId { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Flight Name cannot be longer than 100 characters.")]
    public string FlightName { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Seat Capacity must be a positive number.")]
    public int SeatCapacity { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Start Location cannot be longer than 100 characters.")]
    public string StartLocation { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Destination cannot be longer than 100 characters.")]
    public string Destination { get; set; }

    [Required]
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly TravelDate { get; set; }  

    [Required]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly ArrivalTime { get; set; }  

    [Required]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly DepartureTime { get; set; }  

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Fare must be greater than or equal to zero.")]
    public double Fare { get; set; }

    // Custom validation logic
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Start Location and Destination should not be the same
        if (StartLocation.Equals(Destination, StringComparison.OrdinalIgnoreCase))
        {
            yield return new ValidationResult("Start Location and Destination should not be the same.", new[] { nameof(StartLocation), nameof(Destination) });
        }

        // Travel date should not be in the past
        if (TravelDate < DateOnly.FromDateTime(DateTime.Now))
        {
            yield return new ValidationResult("Travel Date should not be in the past.", new[] { nameof(TravelDate) });
        }

        // Arrival time should be after departure time
        if (DepartureTime <= ArrivalTime)
        {
            yield return new ValidationResult("Arrival Time should be after Departure Time.", new[] { nameof(ArrivalTime), nameof(DepartureTime) });
        }
    }
}

}


