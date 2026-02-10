using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros.Interface
{
    public interface IConciliadoraDashBoardService
    {
        Task<ConciliadoraAuthResponse> LoginAsync(string username, string password);
        Task<ConciliadoraDashboardVendaResponseModel> GetVendasAsync(string token, ConciliadoraDashBoardVendasRequest model);
        Task<ConciliadoraDashboardVendaResponseModel> GetPagamentosAsync(string token, ConciliadoraDashBoardVendasRequest model);
        Task<ConciliadoraDashboardDebitosResponseModel> GetDebitosAsync(string token, ConciliadoraDashBoardVendasRequest model);
        Task<ConciliadoraDashboardTaxaResponse> GetTaxasAsync(string token, ConciliadoraDashBoardVendasRequest model);
        Task<ConciliadoraDashboardInformacoesComplementaresResponse> GetInformacoesComplementaresAsync(string token, ConciliadoraDashBoardVendasRequest model);
    }
}
