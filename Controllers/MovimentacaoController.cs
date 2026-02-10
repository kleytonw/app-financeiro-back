using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using ERP.Models;
using ERP_API.Models;


namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MovimentacaoController : ControllerBase
    {
        protected Context context;
        public MovimentacaoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Movimentacao
                  .Select(m => new
                  {
                      m.IdMovimentacao,
                      m.NumeroNF,
                      m.Modelo,
                      m.EmitenteCNPJ,
                      m.EmitenteNome,
                      m.DataHoraEmissao,
                      m.DestinatarioCNPJ,
                      m.DestinatarioNome,
                      m.ValorNF
                  }).Take(500).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model )
        {
            var result = context.Movimentacao.AsQueryable();
            switch (model.Chave)
            {
                case "NumeroNotaFiscal":
                    result = result.Where(x => x.NumeroNF == model.Valor);
                    break;
                case "Emitente":
                    result = result.Where(x => x.EmitenteNome.Contains(model.Valor.ToUpper()));
                    break;
                case "DataEmissao":
                    var dataHoraEmissao = Convert.ToDateTime(model.Valor); 
                    result = result.Where(x => x.DataHoraEmissao.Value.Date == dataHoraEmissao.Date);
                    break;
                default:
                    // code block
                    break;
            }

            return Ok(result.Select(m => new
            {
                m.IdMovimentacao,
                m.NumeroNF,
                m.Modelo,
                m.EmitenteCNPJ,
                m.EmitenteNome,
                m.DataHoraEmissao,
                m.DestinatarioCNPJ,
                m.DestinatarioNome,
                m.ValorNF
            }).Take(500).ToList());

            //.Select(m => new
            //{
            //  m.IdMovimentacao,
            // m.NumeroNF,
            // m.Modelo,
            //m.EmitenteCNPJ,
            // m.EmitenteNome,
            // m.DestinatarioCNPJ,
            // m.DestinatarioNome,
            // m.ValorNF
            //}).ToList();
            return Ok(result);
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var movimentacao = context.Movimentacao.FirstOrDefault(x => x.IdMovimentacao == id);
            if (movimentacao == null)
                return BadRequest("Movimentação não encontrada ");

            return Ok(new MovimentacaoResponse()
            {
                VersaoNfe = movimentacao.VersaoNfe,
                IdMovimentacao = movimentacao.IdMovimentacao,
                IdEmpresa = movimentacao.IdEmpresa,
                TipoMovimentacao = movimentacao.TipoMovimentacao,
                CodigoUF = movimentacao.CodigoUF,
                NaturezaOperacao = movimentacao.NaturezaOperacao,
                IndicadorFormaPagamento = movimentacao.IndicadorFormaPagamento,
                Modelo = movimentacao.Modelo,
                Serie = movimentacao.Serie,
                NumeroNF = movimentacao.NumeroNF,
                ChaveAcesso = movimentacao.ChaveAcesso,
                DataHoraEmissao = movimentacao.DataHoraEmissao,
                DataHoraSaiEntrada = movimentacao.DataHoraSaiEntrada,
                EmitenteCNPJ = movimentacao.EmitenteCNPJ,
                EmitenteNome = movimentacao.EmitenteNome,
                EmitenteFantasia = movimentacao.EmitenteFantasia,
                EmitenteIE = movimentacao.EmitenteIE,
                EmitenteIEST = movimentacao.EmitenteIEST,
                EmitenteCRT = movimentacao.EmitenteCRT,
                EmitenteLogradouro = movimentacao.EmitenteLogradouro,
                EmitenteNumero = movimentacao.EmitenteNumero,
                EmitenteBairro = movimentacao.EmitenteBairro,
                EmitenteCodigoMunicipio = movimentacao.EmitenteCodigoMunicipio,
                EmitenteUF = movimentacao.EmitenteUF,
                EmitenteCEP = movimentacao.EmitenteCEP,
                EmitenteCodigoPais = movimentacao.EmitenteCodigoPais,
                EmitentePais = movimentacao.EmitentePais,
                DestinatarioCNPJ = movimentacao.DestinatarioCNPJ,
                DestinatarioCPF = movimentacao.DestinatarioCPF,
                DestinatarioNome = movimentacao.DestinatarioNome,
                DestinatarioEmail = movimentacao.DestinatarioEmail,
                DestinatarioLogradouro = movimentacao.DestinatarioLogradouro,
                DestinatarioNumero = movimentacao.DestinatarioNumero,
                DestinatarioBairro = movimentacao.DestinatarioBairro,
                DestinatarioMunicipio = movimentacao.DestinatarioMunicipio,
                DestinatarioCodigoMunicipio = movimentacao.DestinatarioCodigoMunicipio,
                DestinatarioUF = movimentacao.DestinatarioUF,
                DestinatarioCEP = movimentacao.DestinatarioCEP,
                DestinatarioCodigoPais = movimentacao.DestinatarioCodigoPais,
                DestinatarioPais = movimentacao.DestinatarioPais,
                Total = movimentacao.Total,
                ValorICMS = movimentacao.ValorICMS,
                ValorBC = movimentacao.ValorBC,
                ValorBCST = movimentacao.ValorBCST,
                ValorST = movimentacao.ValorST,
                ValorProdutos = movimentacao.ValorProdutos,
                ValorFrete = movimentacao.ValorFrete,
                ValorSeguro = movimentacao.ValorSeguro,
                ValorDesconto = movimentacao.ValorDesconto,
                ValorVLL = movimentacao.ValorVLL,
                ValorIPI = movimentacao.ValorIPI,
                ValorPIS = movimentacao.ValorPIS,
                ValorCofins = movimentacao.ValorCofins,
                ValorOutro = movimentacao.ValorOutro,
                ValorNF = movimentacao.ValorNF

            });
        }

        [HttpGet]
        [Route("obterItem")]
        public IActionResult ObterItem(int id)
        {
            var result = context.MovimentacaoItem.Where(x => x.IdMovimentacao ==  id)
                 .Select(m => new
                 {
                     m.CodigoProd,
                     m.CodigoEAN,
                     m.NomeProduto,
                     m.NCM,
                     m.CFOP,
                     m.Unidade,
                     m.Quantidade,
                     m.ValorUnitario,
                     m.SubTotal,
                 }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("obterFatura")]
        public IActionResult ObterFatura(int id)
        {
           
            return Ok(null);
        }
    }
}
