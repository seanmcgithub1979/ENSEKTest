using System.Threading.Tasks;

namespace ENSEK.Metering.Repositories.Interfaces
{
    public interface ISqlRepository
    {
        Task<TOuput> GetAsync<TOuput>(DefaultStoredProcedureRequest request);

        Task<int> ExecuteAysnc(DefaultStoredProcedureRequest request);
    }
}