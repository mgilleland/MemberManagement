using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MemberManagement.Api.Features.Members;
using Xunit;

namespace MemberManagement.UnitTests
{
    public class MemberTests
    {
        [Fact]
        public void Member_IsValid()
        {
            var member = GetValidModel();

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.True(actual);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void Member_UserName_IsRequired()
        {
            var member = new MemberModel();

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("UserName")));
        }

        [Fact]
        public void Member_UserName_MinLength_4()
        {
            var member = GetValidModel();
            member.UserName = "123";

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("UserName")));
        }

        [Fact]
        public void Member_UserName_MaxLength_12()
        {
            var member = GetValidModel();
            member.UserName = "1234567890123";

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("UserName")));
        }

        [Fact]
        public void Member_FirstName_IsRequired()
        {
            var member = new MemberModel();

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("FirstName")));
        }

        [Fact]
        public void Member_FirstName_MinLength_2()
        {
            var member = GetValidModel();
            member.FirstName = "1";

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("FirstName")));
        }

        [Fact]
        public void Member_FirstName_MaxLength_100()
        {
            var member = GetValidModel();
            member.FirstName = new string('x', 101);

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("FirstName")));
        }

        [Fact]
        public void Member_LastName_IsRequired()
        {
            var member = new MemberModel();

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("LastName")));
        }

        [Fact]
        public void Member_LastName_MinLength_2()
        {
            var member = GetValidModel();
            member.LastName = "1";

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("LastName")));
        }

        [Fact]
        public void Member_LastName_MaxLength_100()
        {
            var member = GetValidModel();
            member.LastName = new string('x', 101);

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("LastName")));
        }

        [Fact]
        public void Member_Email_IsRequired()
        {
            var member = new MemberModel();

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("Email")));
        }

        [Fact]
        public void Member_Email_MaxLength_76()
        {
            var member = GetValidModel();
            member.Email = new string('x', 76);

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("Email")));
        }

        [Fact]
        public void Member_Email_IsWellFormed()
        {
            var member = GetValidModel();
            member.Email = "xxx";

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("Email")));
        }

        [Fact]
        public void Member_PhoneNumber_MaxLength_10()
        {
            var member = GetValidModel();
            member.PhoneNumber = "12345678901";

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("PhoneNumber")));
        }

        [Fact]
        public void Member_PhoneNumber_IsDigits()
        {
            var member = GetValidModel();
            member.PhoneNumber = "123456789x";

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("Phone number must be 10 digits")));
        }

        [Fact]
        public void Member_DateOfBirth_IsRequired()
        {
            var member = new MemberModel();

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("DateOfBirth")));
        }

        [Fact]
        public void Member_DateOfBirth_IsInThePast()
        {
            var member = GetValidModel();
            member.DateOfBirth = DateTime.Now.AddDays(+1);

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

            Assert.False(actual);
            Assert.NotEmpty(validationResults.Where(v => v.ErrorMessage.Contains("Date of Birth cannot be in the future")));
        }

        private static MemberModel GetValidModel()
        {
            var member = new MemberModel()
            {
                UserName = "TestUserName",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "email@test.com",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.AddDays(-1)
            };

            return member;
        }
    }
}
