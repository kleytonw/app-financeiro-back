using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using ERP.Models;
using ERP.Domain.Entidades;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ERP.Infra;
using ERP.Domain.ModelSerialization;
using Newtonsoft.Json;
using System.Data.Entity.Validation;
using ERP_API.Serializable;

namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ImportacaoController : ControllerBase
    {
        protected Context context;
        public ImportacaoController(Context context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("enviarArquivos")]
        [AllowAnonymous]
        public async Task<IActionResult> EnviarArquivo([FromForm] FileProvider fileProvider)
        {
            if(fileProvider.Files==null)
            {
                return BadRequest("Selecione um arquivo");
            }

            var arquivos = fileProvider.Files;
            var tipoMovimentacao = fileProvider.TipoMovimentacao;
            int idEmpresa = fileProvider.IdEmpresa;
            long tamanhoArquivos = arquivos.Sum(f => f.Length);

            // caminho completo do arquivo na localização temporária
            var caminhoArquivo = Path.GetTempFileName();

            //percorre a lista de arquivos selecionados
            var listaMovimentacao = new List<Movimentacao>();
            foreach (var arquivo in arquivos)
            {
                using (var reader = new StreamReader(arquivo.OpenReadStream()))
                {
                    string x = reader.ReadToEnd();
                    var nfe = SerializationHelper.DeserializeXml<NFeProc>(x);

                    var movimentacao = context.Movimentacao.FirstOrDefault(x => x.NumeroNF == nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.nNF); // Adicionar Empresa 
                    if (movimentacao == null)
                        MapearMovimentacaoNfe(listaMovimentacao, nfe,tipoMovimentacao, idEmpresa);
                    else
                        continue;
                }
            }
            if(listaMovimentacao.Count > 0)
            {
                context.Movimentacao.AddRange(listaMovimentacao);
                context.SaveChanges();
            }
            return Ok();
        }

        private static void MapearMovimentacaoNfe(List<Movimentacao> listaMovimentacao, NFeProc nfe, string tipoMovimentacao, int idEmpresa)
        {
            var itemMovimentacao = new Movimentacao();
            itemMovimentacao.VersaoNfe = nfe.versao;
            itemMovimentacao.SetIdentificacao(
                codigoUF: nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.cUF,
                codigoNF: nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.cNF,
                naturezaOperacao: nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.natOp,
                indicadorFormaPagamento: nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.indPag,
                modelo: nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.mod,
                serie: nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.serie,
                numeroNF: nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.nNF,
                dataHoraEmissao: nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.dhEmi,
                dataHoraSaiEntrada: nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.dhSaiEnt,
                chaveAcesso: nfe.ProtNFe.InfProt.ChaveAcesso,
                tipoMovimentacao: tipoMovimentacao,
                idEmpresa: idEmpresa
                );

            AdicionarEmitente(nfe, itemMovimentacao);
            AdicionarDestinatario(nfe, itemMovimentacao);
            AdicionarItens(nfe, itemMovimentacao);
            AdicionarTotal(nfe, itemMovimentacao);
            // AdicionarDadosFatura(nfe, itemMovimentacao);
            AdicionarDadosDuplicata(nfe, itemMovimentacao);

            itemMovimentacao.SetUsuarioInclusao("nfe api");
            listaMovimentacao.Add(itemMovimentacao);
        }
        private static void AdicionarDadosDuplicata(NFeProc nfe, Movimentacao itemMovimentacao)
        {
            var dadosDuplicata = nfe.NotaFiscalEletronica.InformacoesNFe?.Cobranca?.Duplicata;
            if (dadosDuplicata != null)
            {
                foreach (var item in dadosDuplicata)
                {
                    itemMovimentacao.AddDuplicada(new MovimentacaoDuplicata(item.nDup, item.dVenc, item.vDup));
                }
            }
        }
        /* private static void AdicionarDadosFatura(NFeProc nfe, Movimentacao itemMovimentacao)
        {
            var dadosFatura = nfe.NotaFiscalEletronica.InformacoesNFe?.Cobranca?.Fatura;
            if(dadosFatura!=null)
                itemMovimentacao.Fatura = new MovimentacaoFatura(dadosFatura.vOrig, dadosFatura.vDesc, dadosFatura.vOrig);
        } */
        private static void AdicionarDestinatario(NFeProc nfe, Movimentacao itemMovimentacao)
        {
            var dadosDestinatario = nfe.NotaFiscalEletronica.InformacoesNFe?.Destinatario;
            if (dadosDestinatario != null) // modelo 65
            {
                itemMovimentacao.SetDadosDestinatario(destinatarioCNPJ: dadosDestinatario.CNPJ, destinatarioCPF: dadosDestinatario.CPF, destinatarioNome: dadosDestinatario.xNome, destinatarioEmail: dadosDestinatario.email);

                var enderecoDestinatario = nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco;
                itemMovimentacao.SetEnderecoDestinatario(destinatarioLogradouro: enderecoDestinatario.xLgr,
                    destinatarioNumero: enderecoDestinatario.nro,
                    destinatarioBairro: enderecoDestinatario.xBairro,
                    destinatarioCodigoMunicipio: enderecoDestinatario.cMun,
                    destinatarioMunicipio: enderecoDestinatario.xMun,
                    destinatarioUF: enderecoDestinatario.UF,
                    destinatarioCEP: enderecoDestinatario.CEP,
                    destinatarioCodigoPais: enderecoDestinatario.cPais,
                    destinatarioPais: enderecoDestinatario.xPais);
            }
        }
        private static void AdicionarEmitente(NFeProc nfe, Movimentacao itemMovimentacao)
        {
            itemMovimentacao.SetDadosEmitente(emitenteCNPJ: nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CNPJ,
                emitenteNome: nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xNome,
                emitenteFantasia: nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xFant,
                emitenteIE: nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.IE,
                emitenteIEST: nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.IEST,
                emitenteCRT: nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CRT);

            var enderecoEmitente = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco;
            itemMovimentacao.SetEnderecoEmitente(emitenteLogradouro: enderecoEmitente.xLgr,
                emitenteNumero: enderecoEmitente.nro,
                emitenteBairro: enderecoEmitente.xBairro,
                emitenteCodigoMunicipio: enderecoEmitente.cMun,
                emitenteMunicipio: enderecoEmitente.xMun,
                emitenteUF: enderecoEmitente.UF,
                emitenteCEP: enderecoEmitente.CEP,
                emitenteCodigoPais: enderecoEmitente.cPais,
                emitentePais: enderecoEmitente.xPais);
        }
        private static void AdicionarItens(NFeProc nfe, Movimentacao itemMovimentacao)
        {
            var produtos = nfe.NotaFiscalEletronica.InformacoesNFe.Detalhe;
            foreach (var produto in produtos)
            {
                itemMovimentacao.AddItem(new MovimentacaoItem(
                     codigoProd: produto.Produto.cProd,
                     codigoEAN: produto.Produto.cEAN,
                     nomeProduto: produto.Produto.xProd,
                     nCM: produto.Produto.NCM,
                     cFOP: produto.Produto.CFOP,
                     unidade: produto.Produto.uCom,
                     quantidade: produto.Produto.qCom,
                     valorUnitario: produto.Produto.vUnCom));
            }
        }
        private static void AdicionarTotal(NFeProc nfe, Movimentacao itemMovimentacao)
        {
            var dadosTotais = nfe.NotaFiscalEletronica.InformacoesNFe.Total.ICMSTot;
            itemMovimentacao.SetTotal(
                total: dadosTotais.vNF,
                valorICMS: dadosTotais.vICMS,
                valorBC: dadosTotais.vBC,
                valorBCST: dadosTotais.vBCST,
                valorST: dadosTotais.vST,
                valorProdutos: dadosTotais.vProd,
                valorFrete: dadosTotais.vFrete,
                valorSeguro: dadosTotais.vSeg,
                valorDesconto: dadosTotais.vDesc,
                valorvII: dadosTotais.vII,
                valorPIS: dadosTotais.vPIS,
                valorIPI: dadosTotais.vIPI,
                valorCofins: dadosTotais.vCOFINS,
                valorOutro: dadosTotais.vOutro,
                valorNF: dadosTotais.vNF,
                valorTotalTributos: dadosTotais.vTotTrib);
        }
    }
}

public class FileProvider
{
    public int IdEmpresa { get; set;  }
    public string TipoMovimentacao { get; set; }
    public IFormCollection FormData { get; set; }
    public IList<IFormFile> Files { get; set; }
}
