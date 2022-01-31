using System.Collections.Generic;
using System.Data;
using ENSEK.Metering.Domain.Models;
using ENSEK.Metering.Repositories.Interfaces;

namespace ENSEK.Metering.Repositories
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly ISqlRepository _sqlRepository;

        public MeterReadingRepository(ISqlRepository sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }

        public int Save(AccountMeterReading entity)
        {
            var rowsAffected = _sqlRepository.ExecuteAysnc(new DefaultStoredProcedureRequest
            {
                StoredProcedureName = "dbo.Insert_AccountMeterReading",
                Parameters = new List<StoredProcedureParamsRequest>
                {
                    new StoredProcedureParamsRequest
                    {
                        ParameterName = "@AccountId", ParameterValue = entity.AccountId, DatabaseType = DbType.Int32
                    },
                    new StoredProcedureParamsRequest
                    {
                        ParameterName = "@MeterReadingDateTime", ParameterValue = entity.MeterReadingDateTime, DatabaseType = DbType.DateTime
                    },
                    new StoredProcedureParamsRequest
                    {
                        ParameterName = "@MeterReading", ParameterValue = entity.MeterReadingValue, DatabaseType = DbType.Int32
                    },
                }
            }).Result;

            return rowsAffected;
        }
    }
}
