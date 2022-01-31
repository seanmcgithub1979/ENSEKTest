namespace ENSEK.Metering.Services.Interfaces
{
    public interface IValidationService
    {
        bool IsValidFile(string filename);
        bool IsValidRow(string[] fields);
        bool IsValidReading(int meterReading);
    }
}