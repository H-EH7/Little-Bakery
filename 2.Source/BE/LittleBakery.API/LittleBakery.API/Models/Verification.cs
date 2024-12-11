namespace LittleBakery.API.Models
{
    public class Verification
    {
        public string UUID { get; set; }
        public string Number { get; set; }
    }

    public class VerificationRequest
    {
        public string UUID { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class VerificationNumber
    {
        public string Number { get; set; }
        public DateTime Created { get; }

        public VerificationNumber(string number)
        {
            Number = number;
            Created = DateTime.Now;
        }
    }
}
