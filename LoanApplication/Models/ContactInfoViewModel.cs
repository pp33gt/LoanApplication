using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoanApplication.Models
{

    public class ContactInfoViewModel: ILoanApplication
    {
        [Required(ErrorMessage = "Obligatoriskt fält")]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Obligatoriskt fält")]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        [Display(Name = "E-Post")]
        public string Email { get; set; }

        [Display(Name = "Telefonnummer")]
        [RegularExpression(@"^[0-9]{2,3}-[0-9]+$", ErrorMessage = "Felaktigt telefonnummer")]
        public string PhoneNumber { get; set; }

        //Hidden fields
        public MaritalStatus MaritalStatus { get; set; }
        public Occupation Occupation { get; set; }
        public int YearOfBirth { get; set; }
        public string LoanApplicationViewData { get; set; }
    }
}
