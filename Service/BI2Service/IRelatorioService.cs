using ERP_API.Models.BI2;
using ERP_API.Models.BI2.Filtros;
using System.Collections.Generic;

namespace ERP_API.Service.BI2Service
{
    public interface IRelatorioBIService
    {
        public List<RelatorioFaturamentoMensalModel> ObterFaturamentoMensal(FiltroBIModel model);

        public List<RelatorioMargemBrutaModel> ObterMargemBrutaMensal(FiltroBIModel model);

        public List<RelatorioTicketMedioModel> ObterTicketMedio(FiltroBIModel model);

        public List<RelatorioTopProdutoModel> TopProdutosMaisVendidos(FiltroBIModel model);

        public List<RelatorioDevolucaoModel> ObterDevolucoes(FiltroBIModel model);
        public List<RelatorioMovimentacaoTipoModel> ObterMovimentacaoPorTipo(FiltroBIModel model);

        public List<RelatorioVolumeItensModel> ObterVolumeItens(FiltroBIModel model);

        public List<RelatorioMovimentacaoPeriodoModel> ObterMovimentacaoPeriodo(FiltroBIMovModel model);

        public List<RelatorioTopCategoriaModel> ObterTopCategoria(FiltroBIMovModel model);

        public List<RelatorioFaturamentoClienteModel> FaturamentoCliente(FiltroBIMovModel model);

        public RelatorioPromocionalModel ObterPromocional(FiltroBIModel model);
    }
}
