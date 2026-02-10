using System.Threading.Tasks;
using static ERP_API.Service.Parceiros.ConsultaVendaRedeResponseModel;

namespace ERP_API.Service.Parceiros.Interface
{
    public interface IRedeService
    {
        Task<LoginResponseModel> LoginRedeAsync(LoginRequestModel request);
        Task<ConsultaVendaResponseModel> ConsultaVendaRedeAsync(ConsultarVendaRedeRequestModel request);
        Task<ConsultaPagamentoRedeResponseModel> ConsultaPagamentoRedeAsync(ConsultaPagamentoRedeRequestModel request);
        Task<ConsultarPagamentoDiarioRedeResponseModel> ConsultarPagamentoDiarioRedeAsync(ConsultarPagamentoDiarioRedeRequestModel request);

    }
}
