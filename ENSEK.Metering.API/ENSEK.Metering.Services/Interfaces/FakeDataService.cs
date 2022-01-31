using System.Linq;

namespace ENSEK.Metering.Services.Interfaces
{
    public class FakeDataService : IDataService
    {
        public int Save(string[] rowData)
        {
            var validAccountNumbers = "2345,2351,6776,1239".Split(',');

            if (validAccountNumbers.Contains(rowData[0]))
            {
                return 1;
            }

            return 0;

        }
    }
}