/*using FlightBooking_CaseStudy_sameer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightBooking_CaseStudy_sameer.DTOs
{
    public class UserDTO
    {

        public int UserId { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, MinimumLength = 3,ErrorMessage = "First Name can't be longer than 50 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last Name can't be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("Male|Female|Other", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(User), "ValidateDOB")]
        public DateTime DOB { get; set; }

        [Range(18, 120, ErrorMessage = "Age must be between 18 and 120.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, ErrorMessage = "Username can't be longer than 20 characters.")]
        public string UserName { get; set; }


        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(10, ErrorMessage = "Mobile Number can't be longer than 10 digits.")]
        public string MobileNumber { get; set; }

        // Optionally include bookings if needed
        // public ICollection<BookingDTO> Bookings { get; set; }
    }
}*/
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FlightBooking_CaseStudy_sameer.DTOs
{
    public class UserDTO
    {
       // [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First Name can't be longer than 50 characters and lessthan 3 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Last Name can't be longer than 50 characters and lessthan 3 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("Male|Female|Other", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(UserDTO), "ValidateDOB")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [CustomValidation(typeof(UserDTO), "ValidateAge")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 3,ErrorMessage = "Username can't be longer than 20 characters and lessthan 3 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        [CustomValidation(typeof(UserDTO), "ValidatePassword")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword must match properly")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(15, ErrorMessage = "Mobile Number can't be longer than 15 digits.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Role can't be longer than 20 characters and lessthan 3 characters.")]
        public string Role { get; set; }

        // Custom validation for Date of Birth to ensure age is greater than 18
        public static ValidationResult ValidateDOB(DateTime dob, ValidationContext context)
        {
            if (dob > DateTime.Now)
            {
                return new ValidationResult("Date of Birth cannot be in the future.");
            }

            var age = DateTime.Now.Year - dob.Year;
            if (dob.AddYears(age) > DateTime.Now) age--;

            if (age < 18)
            {
                return new ValidationResult("User must be at least 18 years old.");
            }

            return ValidationResult.Success;
        }

        // Custom validation for Age to be greater than 18
        public static ValidationResult ValidateAge(int age, ValidationContext context)
        {
            if (age <= 18)
            {
                return new ValidationResult("Age must be greater than 18.");
            }
            return ValidationResult.Success;
        }

        // Custom validation for strong password
        public static ValidationResult ValidatePassword(string password, ValidationContext context)
        {
            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$";

            if (!Regex.IsMatch(password, passwordPattern))
            {
                return new ValidationResult("Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
            }

            return ValidationResult.Success;
        }
    }
}
