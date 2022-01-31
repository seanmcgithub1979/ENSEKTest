using System.Collections.Generic;

namespace ENSEK.Metering.Domain.Models
{
    public class MeterReadingUploadResponse
    {
        public IList<MeterReadingUploadResult> MeterReadingUploadResults { get; set; } = new List<MeterReadingUploadResult>();
    }
}
