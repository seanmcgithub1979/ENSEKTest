using System.Data;

namespace ENSEK.Metering.Repositories
{
    public class StoredProcedureParamsRequest
    {
        public string ParameterName { get; set; }

        public object ParameterValue { get; set; }

        public DbType DatabaseType { get; set; }
    }
}