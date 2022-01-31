using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ENSEK.Metering.Repositories.Interfaces;

namespace ENSEK.Metering.Repositories
{
    public class SqlRepository : ISqlRepository
    {
        private readonly string _connectionString;

        public SqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<TOuput> GetAsync<TOuput>(DefaultStoredProcedureRequest request)
        {
            var connection = GenerateDatabaseConnection();
            await using var _ = connection;
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }

            var response = await connection.QueryAsync<TOuput>(
                request.StoredProcedureName, MapParameters(request.Parameters), commandType: CommandType.StoredProcedure);

            return response.FirstOrDefault();
        }

        
        public async Task<int> ExecuteAysnc(DefaultStoredProcedureRequest request)
        {
            int result;

            await using var connection = GenerateDatabaseConnection(); ;
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }

            try
            {
                result = await connection
                    .ExecuteAsync(request.StoredProcedureName, MapParameters(request.Parameters), commandType: CommandType.StoredProcedure);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }

            return result;
        }

        private SqlConnection GenerateDatabaseConnection() => new SqlConnection(_connectionString);

        private DynamicParameters MapParameters(IEnumerable<StoredProcedureParamsRequest> parameters)
        {
            if (parameters == null || !parameters.Any())
            {
                return null;
            }

            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.ParameterName, parameter.ParameterValue, parameter.DatabaseType);
            }

            return dynamicParameters;
        }
    }
}