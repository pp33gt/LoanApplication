using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoanApplication.Models
{
    public class SummaryViewModel: ILoanApplication
    {
        [Required]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[a-z,0-9]+@[a-z,0-9]+.[a-z]{2}$")]
        [Display(Name = "E-Post")]
        public string Email { get; set; }

        [Display(Name = "Telefonnummer")]
        [RegularExpression(@"^[0-9]{2,3}-[0-9]+$")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Civilstånd")]
        public MaritalStatus MaritalStatus { get; set; }

        [Required]
        [Display(Name = "Sysselsättning")]
        public Occupation Occupation { get; set; }

        [Required]
        [Display(Name = "Födelseår")]
        [RegularExpression(@"^[0-9]{4}$")]
        public int YearOfBirth { get; set; }

        public string MaritalStatusDisplayName { get; set; }

        public string OccupationDisplayName { get; set; }
    }
}
