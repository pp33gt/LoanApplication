using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoanApplication.Models
{
    public class LoanApplicationViewModel : ILoanApplication
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        //[RegularExpression(@"^[a-z,0-9]+.[a-z,0-9]+@[a-z,0-9]+.[a-z]{2}$")]
        //[EmailAddress] //(ErrorMessage = "Invalid Email Address")
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [RegularExpression(@"^[0-9]{2,3}-[0-9]+$", ErrorMessage = "Felaktigt telefonnummer")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Civilstånd")]
        public MaritalStatus MaritalStatus { get; set; }

        public string MaritalStatusValidationMsg { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Sysselsättning")]
        public Occupation Occupation { get; set; }

        public string OccupationValidationMsg { get; set; } = string.Empty;

        public int YearOfBirth { get; set; }

        [Display(Name = "Födelseår")]
        [RegularExpression(@"^[0-9]{4}$")]
        public string YearOfBirthString { get; set; }

        public string YearOfBirthValidationMsg { get; set; } = string.Empty; 

        public List<SelectListItem> Occupations { get; set; }

        public List<SelectListItem> MaritalStatuses { get; set; }
    }
}
