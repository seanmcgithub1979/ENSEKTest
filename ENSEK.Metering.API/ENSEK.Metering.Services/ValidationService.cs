using System;
using System.IO;
using ENSEK.Metering.Services.Interfaces;

namespace ENSEK.Metering.Services
{
    public class ValidationService : IValidationService
    {
        public bool IsValidFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return false;
            }

            var fi = new FileInfo(filename);
            
            return fi.Extension == ".csv";
        }

        public bool IsValidRow(string[] fields)
        {
            if (int.TryParse(fields[0], out _) && DateTime.TryParse(fields[1], out _) && int.TryParse(fields[2], out _))
            {
                return true;
            }

            return false;
        }

        public bool IsValidReading(int meterReading)
        {
            return meterReading >= 10000 && meterReading <= 99999;
        }
    }
}
