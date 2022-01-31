using System;

namespace ENSEK.Metering.Domain.Models
{
    public class AccountMeterReading : BaseEntity
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadingValue { get; set; } //TODO: Invalid data
    }
}
