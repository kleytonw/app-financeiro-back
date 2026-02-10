using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros.Interface
{
    public interface ITecnospeedService
    {
        Task<CadastroPagadorResponseModel> CadastroPagadorTecnospeed(CadastroPagadorRequestModel request, string cnpjsh, string tokensh, string url);
        Task<CadastroPagadorResponseModel> BuscarPagadorTecnospeed(string cnpjPagador, string cnpjsh, string tokensh, string url);
        Task<AtualizarPagadorResponseModel> AtualizarPagadorTecnospeed(AtualizarPagadorRequestModel request, Unidade unidade, string cnpjsh, string tokensh, string url);
        Task<CadastrarContaListResponseModel> CadastrarContaTecnospeed(CadastrarContaRequestListModel request, Unidade unidade, string cnpjsh, string tokensh, string url);
        Task<AtualizarContaResponseModel> AtualizarContaTecnospeed(AtualizarContaRequestModel request, Unidade unidade, string hashDaConta, string cnpjsh, string tokensh, string url);
        Task<DeletarContaResponseModel> DeletarContaTecnospeed(DeletarContaRequestModel request, Unidade unidade, string cnpjsh, string tokensh, string url);
        Task<BilhetagemTecnospeedResponseModel> BilhetagemTecnospeed(BilhetagemTecnospeedRequestModel request, string cnpjsh, string tokensh, string url);
        Task<CriarNotificacaoTecnospeedResponseModel> CriarNotificacaoTecnospeed(CriarNotificacaoTecnospeedRequestModel request, Unidade unidade, string cnpjsh, string tokensh, string url);
        Task<ListarNotificacaoTecnospeedResponseModel> ListarNotificacaoTecnospeed( Unidade unidade, string cnpjsh, string tokensh, string url);
        Task<DeletarNotificacaoTecnospeedResponseModel> DeletarNotificacaoTecnospeed(Unidade unidade, string cnpjsh, string tokensh, string url);
        Task<EnviarExtratoTecnospeedResponseModel> EnviarExtratoTecnospeed(EnviarExtratoTecnospeedRequestModel request, Unidade unidade, string cnpjsh, string tokensh, string url);
        Task<ConsultarExtratoTecnospeedResponseModel> ConsultarExtratoTecnospeed(Unidade unidade, Extrato extrato, string cnpjsh, string tokensh, string url);
        Task<ConsultarExtratoPorPeriodoTecnospeedResponseModel> ConsultarExtratoPorPeriodoTecnospeed(ConsultarExtratoPorPeriodoTecnospeedRequestModel request, Unidade unidade, string cnpjsh, string tokensh, string url);
        Task<byte[]> BaixarExtratoTecnospeed(Unidade unidade, string cnpjsh, string tokensh, string url);
    }
}
