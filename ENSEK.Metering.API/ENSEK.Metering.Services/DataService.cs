using System;
using ENSEK.Metering.Domain.Models;
using ENSEK.Metering.Repositories.Interfaces;
using ENSEK.Metering.Services.Interfaces;

namespace ENSEK.Metering.Services
{
    public class DataService : IDataService
    {
        private readonly IMeterReadingRepository _meterReadingRepository;

        public DataService(IMeterReadingRepository meterReadingRepository)
        {
            _meterReadingRepository = meterReadingRepository;
        }

        public int Save(string[] rowData)
        {
            var accountMeterReading = new AccountMeterReading
            {
                AccountId = int.Parse(rowData[0]),
                MeterReadingDateTime = DateTime.Parse(rowData[1]),
                MeterReadingValue = int.Parse(rowData[2])
            };

            return _meterReadingRepository.Save(accountMeterReading);
        }
    }
}
