using ERP_API.Models;
using ERP_API.Models.NotaFiscal;
using ERP_Application.Models;
using ERP_Application.Models.Parceiros.IntegraNotas;
using System.Threading.Tasks;

namespace ERP_Application.Services.Parceiros.IntegraNotas.Interface
{
    public interface IIntegraNotasService
    {
        Task <Response<CancelarNFseResponseModel>> CancelarNFseAsync(CancelarNFseRequestModel pCancelarRequestModel, string _token);

        //Task<Response<ConsultarNFseResponseModel>> ConsultarNFseAsync(ConsultarNFseRequestModel pConsultarNFseRequestModel);
        Task <Response<ConsultarNFseResponseModel>> ConsultarNFseAsync(string chave, string _token);
        Task <Response<PdfNFseResponseModel>> GerarPdfNFseAsync(string chave, string _token);
        Task<Response<CriarEmitenteIntegraNotasResponseModel>> CriarEmitenteAsync(CriarEmitenteIntegraNotasRequestModel pEmitenteRequestModel, string _token);

        Task<Response<NfseResponseModel>> CriarNFseAsync(NfseRequestRequestModel pCriarNfse, string _token);

        Task<Response<BuscarNFseResponseModel>> BuscarNFseAsync(BuscarNfseRequestModel pBuscaNfseRequestModel, string _token);
    }
}
