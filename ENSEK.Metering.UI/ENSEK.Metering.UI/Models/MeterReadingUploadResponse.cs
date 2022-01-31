using System.Collections.Generic;

namespace ENSEK.Metering.UI.Models
{
    public class MeterReadingUploadResponse
    {
        public MeterReadingUploadResponse()
        {
            _meterReadingUploadResults.Add(new MeterReadingUploadResult {Status = "SUCCESS"});
            _meterReadingUploadResults.Add(new MeterReadingUploadResult {Status = "FAILURE"});
        }
        private readonly List<MeterReadingUploadResult> _meterReadingUploadResults = new List<MeterReadingUploadResult>();
        public IList<MeterReadingUploadResult> MeterReadingUploadResults { get; set; }
    }
}
