/*using System.Collections.Generic;

namespace FlightBooking_CaseStudy_sameer.DTO
{
    public class AdminDTO
    {
        public int AdminId { get; set; }
        public string AdminName { get; set; }   
         public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
    }
}
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FlightBooking_CaseStudy_sameer.DTO
{
    
    public class AdminDTO
    {
        [Required(ErrorMessage = "Admin ID is required.")]
        public int AdminId { get; set; }

        [Required(ErrorMessage = "Admin Name is required.")]
        [StringLength(100, ErrorMessage = "Admin Name cannot be longer than 100 characters.")]
        public string AdminName { get; set; }

        [Required(ErrorMessage = "Admin Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(256, ErrorMessage = "Admin Email cannot be longer than 256 characters.")]
        public string AdminEmail { get; set; }

        [Required(ErrorMessage = "Admin Password is required.")]
        [StrongPassword]  // Apply the custom strong password validation
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Admin Password must be between 8 and 100 characters.")]
        public string AdminPassword { get; set; }
        [Required(ErrorMessage = "Role Name is required.")]
        [StringLength(100, ErrorMessage = "Role Name cannot be longer than 100 characters.")]
        public string Role {  get; set; }
        //public int FailedLoginAttemptse { get; set; }
    }
    public class StrongPasswordAttribute : ValidationAttribute
    {
        private const string PasswordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$";

        public StrongPasswordAttribute()
            : base("Password must be at least 8 characters long, include at least one letter, one number, and one special character.")
        { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrWhiteSpace(password) || !Regex.IsMatch(password, PasswordPattern))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
