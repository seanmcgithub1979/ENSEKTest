using System.Collections.Generic;

namespace ENSEK.Metering.Repositories
{
    public class DefaultStoredProcedureRequest
    {
        public string StoredProcedureName { get; set; }

        public IEnumerable<StoredProcedureParamsRequest> Parameters { get; set; } = new List<StoredProcedureParamsRequest>();
    }
}