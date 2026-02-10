using ERP_API.Models;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros.Interface
{
    public interface INexxeraService
    {
        Task<ArquivoListagemNexxeraResponse> ListagemArquivos(ListagemArquivosNexxeraRequest model);
        Task<RedisponibilizarArquivoNexxeraResponse> RedisponibilizarArquivo(RedisponibilizarArquivoNexxeraRequest model);
        Task<DownloadNexxeraResponse> Download(DownloadNexxeraRequest model);
        Task<UploadNexxeraResponse> Upload(UploadNexxeraRequest model);
    }
}
