using static ERP_API.Service.Parceiros.ConsultaVendaRedeResponseModel;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros.Interface
{
    public interface IPagBankService
    {
        Task<ConsultaTransacaoPagBankResponseModel> ConsultaTransacaoPagBankAsync(ConsultaPagBankRequestModel request);
        Task<ConsultaPagamentoPagBankResponseModel> ConsultaPagamentoPagBankAsync(ConsultaPagBankRequestModel request);
        Task<ConsultaCashBackPagBankResponseModel> ConsultaCashBackPagBankAsync(ConsultaPagBankRequestModel request);


    }
}
