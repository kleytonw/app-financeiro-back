using Azure;
using ERP_API.Models.NotaFiscal;
using System.Net.Http;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros.Interface
{
    public interface IUniqueService
    {
        Task<string> GerarAccessTokenAsync(string login, string senha, string url);

        Task<CriarCobrancaResponse> CriarCobrancaAsync(CobrancaRequest request, string token, string url);
        Task<HttpResponseMessage> ConsultarCobrancaAsync(int idTransacao, string token, string url);
        Task<HttpResponseMessage> CancelarCobrancaAsync(int? idTransacao, string token, string url);
        Task<HttpResponseMessage> ConsultarCobrancasAsync(FiltroCobrancaRequest request);

        Task<HttpResponseMessage> DepositarAsync(DepositoRequest request);


        Task<NotaFiscalResponseModel> CriarNfeAsync(CriarNotaFiscalRequestModel request, string token, string usuarioInclusao);
        Task<NotaFiscalResponseModel> ConsultaNfeAsync(int idNotaFiscal, string token, string usuarioLogado);
        Task<NotaFiscalResponseModel> ExcluirNfeAsync(int idNotaFiscal, string token, string usuarioExclusao);

    }
}
