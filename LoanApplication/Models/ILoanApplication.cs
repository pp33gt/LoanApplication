namespace LoanApplication.Models
{
    public interface ILoanApplication
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string Email { get; set; }

        string PhoneNumber { get; set; }

        MaritalStatus MaritalStatus { get; set; }

        Occupation Occupation { get; set; }

        int YearOfBirth { get; set; }
    }
}
