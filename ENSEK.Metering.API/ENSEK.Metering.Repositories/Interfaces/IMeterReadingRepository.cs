using ENSEK.Metering.Domain.Models;

namespace ENSEK.Metering.Repositories.Interfaces
{
    public interface IMeterReadingRepository
    {
        public int Save(AccountMeterReading entity);
    }
}
