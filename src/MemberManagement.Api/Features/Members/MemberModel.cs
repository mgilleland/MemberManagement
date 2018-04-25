using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MemberManagement.Api.Features.Members
{
    public class MemberModel : IValidatableObject
    {
        [Required]
        [MinLength(4)]
        [MaxLength(12)]
        public string UserName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(75)]
        public string Email { get; set; }

        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // If the phone number is supplied, validate that it is 10 digits
            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                Regex phonePattern = new Regex(@"(?<!\d)\d{10}(?!\d)");
                if (!phonePattern.IsMatch(PhoneNumber))
                {
                    yield return new ValidationResult("Phone number must be 10 digits", new List<string>{"PhoneNumber"});
                }
            }

            // DOB cannot be in the future
            if (DateOfBirth != null && DateOfBirth.Value.Date > DateTime.Now.Date)
            {
                yield return new ValidationResult("Date of Birth cannot be in the future", new List<string> { "DateOfBirth" });
            }
        }
    }
}
