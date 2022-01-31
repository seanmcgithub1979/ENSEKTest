namespace ENSEK.Metering.Domain.Models
{
    public class Account : BaseEntity
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
