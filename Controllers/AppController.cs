using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using System;
using System.Linq; 
using ERP_API.Domain.Entidades;
using System.Data.Entity;
using System.Threading.Tasks;
using ERP.Models.SecurityToken;
using ERP_API.Models.App;
using ERP_API.Service;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Microsoft.AspNetCore.Http;
using System.Data;
using ERP_API.Service.Parceiros.Interface;
using System.Net.Http;
using ERP_API.Models;
using ERP_API.Models.MovimentacaoDiaria;
using System.Collections.Generic;
using System.Globalization;


namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/app")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class AppController : ControllerBase
    {

        protected Context context;
        protected IConciliadoraService _conciliadoraService;
        private readonly HttpClient _httpClient;
        private IBlobStorageService blobStorageService;
        private readonly string _baseUrl = "https://api.conciliadora.com.br/api/EnvioVendaSistema";
        public AppController(Context context, IConciliadoraService conciliadoraService, HttpClient httpClient, IBlobStorageService blobStorageService)
        {
            this.context = context;
            this._conciliadoraService = conciliadoraService;
            this._httpClient = httpClient;
            this.blobStorageService = blobStorageService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> AuthenticateAsync([FromBody] LoginAppModel model)
        {
            try
            {
                var usuario = context.Usuario.FirstOrDefault(x => x.Login == model.Login && x.Senha == model.Senha);
                if (usuario == null)
                    return BadRequest("Dados de login inválidos");
                if (usuario.Situacao != "Ativo")
                {
                    return BadRequest("Usuário inativo! Por favor, entre em contato com o suporte");
                }
                 
                var token = TokenService.GenerateTokenApp(new Models.SecurityToken.User()
                {
                    Login = model.Login,
                    Grant_Type = "Passwoard",
                    Role = usuario.TipoUsuario,
                    Password = model.Senha,
                    Username = model.Login,
                    Id = 0
                });

                var result = new TokenReturAppModel()
                {
                    Token = token
                };

                return result;
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("produtos")]
        public async Task<IActionResult> GetProdutos(int idCliente, int? top, int? skip)
        {

            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == idCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");



            var (success, errorMessage, data) = await _conciliadoraService.Produto(cliente.ApiKeyConciliadora, top, skip);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }


        [HttpGet]
        [Route("meios-captura")]
        public async Task<IActionResult> GetMeiosCaptura(int idCliente, int? skip, int? top)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == idCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await _conciliadoraService.MeioCaptura(cliente.ApiKeyConciliadora, top, skip);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);

        }



        [HttpGet]
        [Route("adquirentes")]
        public async Task<IActionResult> GetAdquirentes(int idCliente, int? top, int? skip)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == idCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await _conciliadoraService.ListaAdquirenteConciliadoraResponse1(cliente.ApiKeyConciliadora, top, skip);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }


        [HttpGet]
        [Route("modalidades")]
        public async Task<IActionResult> GetModalidades(int idCliente, int ?top, int? skip)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == idCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await _conciliadoraService.Modalidade(cliente.ApiKeyConciliadora, top, skip);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }


        [HttpPost]
        [Route("envio-vendas-cartao-credito")]
        public async Task<IActionResult> EnvioVendaCartaoCredito([FromForm] int idCliente, [FromForm] string senha, IFormFile file)
        {
            try
            {
                var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name && x.TipoUsuario == "ERP");
                if (usuarioLogado == null)
                {
                    return BadRequest("Usuário não encontrado.");
                }

                var cliente = context.Cliente.Include(x => x.ERPs).FirstOrDefault(x => x.IdentificadorConciliadora == idCliente.ToString());
                if (cliente == null)
                {
                    return BadRequest("Cliente não encontrado.");
                }

                var erps = context.ERPs.FirstOrDefault(x => x.IdERPs == usuarioLogado.IdERPs);
                if (erps == null)
                {
                    return BadRequest("ERP não encontrado.");
                }

                if (file == null || file.Length == 0)
                {
                    return BadRequest("Nenhum arquivo foi enviado.");
                }

                var extensao = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (extensao != ".xml")
                {
                    return BadRequest("Apenas arquivos XML são permitidos.");
                }

                string conteudoXml;
                XDocument xmlDoc;
                try
                {
                    using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                    {
                        conteudoXml = await reader.ReadToEndAsync();
                        xmlDoc = XDocument.Parse(conteudoXml);
                    }
                }
                catch (XmlException)
                {
                    return BadRequest("O arquivo não contém um XML válido.");
                }
                catch (Exception)
                {
                    return BadRequest("Erro ao processar o arquivo.");
                }

                if (string.IsNullOrEmpty(cliente.SenhaConciliadora))
                {
                    return BadRequest("Senha não encontrada para este cliente.");
                }

                if (string.IsNullOrEmpty(cliente.IdentificadorConciliadora))
                {
                    return BadRequest("Cliente não possui identificador da conciliadora configurado.");
                }

                // Processar o XML e extrair os dados
                var vendasConciliadasList = new List<VendasConciliadas>();

                try
                {
                    var cabecalho = xmlDoc.Descendants("cabecalho").FirstOrDefault();
                    var registros = xmlDoc.Descendants("registro");

                    if (cabecalho == null)
                    {
                        return BadRequest("XML inválido: cabeçalho não encontrado.");
                    }

                    // Dados do cabeçalho
                    string empresaCabecalho = cabecalho.Element("Empresa")?.Value;
                    DateTime dataInicial = DateTime.ParseExact(cabecalho.Element("DataInicial")?.Value, "dd/MM/yyyy", null);
                    DateTime dataFinal = DateTime.ParseExact(cabecalho.Element("DataFinal")?.Value, "dd/MM/yyyy", null);
                    string versao = cabecalho.Element("Versao")?.Value;
                    int lote = int.Parse(cabecalho.Element("Lote")?.Value ?? "0");
                    string nomeSistema = cabecalho.Element("NomeSistema")?.Value;

                    // Processar cada registro
                    foreach (var registro in registros)
                    {
                        try
                        {
                            int produto = int.Parse(registro.Element("Produto")?.Value);
                            string descricaoTipoProduto = registro.Element("DescricaoTipoProduto")?.Value;
                            string codigoAutorizacao = registro.Element("CodigoAutorizacao")?.Value;
                            string identificadorPagamento = registro.Element("IdentificadorPagamento")?.Value;

                            DateTime dataVenda = DateTime.ParseExact(registro.Element("DataVenda")?.Value, "dd/MM/yyyy", null);
                            DateTime dataVencimento = DateTime.ParseExact(registro.Element("DataVencimento")?.Value, "dd/MM/yyyy", null);

                            decimal valorVendaParcela = decimal.Parse(registro.Element("ValorVendaParcela")?.Value ?? "0", CultureInfo.InvariantCulture);
                            decimal valorVendaLiquidaParcela = decimal.Parse(registro.Element("ValorLiquidoParcela")?.Value ?? "0", CultureInfo.InvariantCulture);
                            decimal valorTotalVendaParcela = decimal.Parse(registro.Element("TotalVenda")?.Value ?? "0", CultureInfo.InvariantCulture);

                            decimal taxa = decimal.Parse(registro.Element("Taxa")?.Value ?? "0", CultureInfo.InvariantCulture);
                            int parcela = int.Parse(registro.Element("Parcela")?.Value ?? "0");
                            int totalParcelas = int.Parse(registro.Element("TotalDeParcelas")?.Value ?? "0");

                            // Campos opcionais ou com valores padrão
                            decimal valorBrutoMoeda = 0; // Se existir no XML, adicionar
                            decimal valorLiquidoMoeda = 0; // Se existir no XML, adicionar
                            decimal cotacaoMoeda = 0; // Se existir no XML, adicionar
                            string moeda = "BRL"; // Valor padrão

                            string nSU = registro.Element("NSU")?.Value;
                            string tID = registro.Element("TID")?.Value;
                            string terminal = registro.Element("Terminal")?.Value;
                            string meioCaptura = registro.Element("MeioCaptura")?.Value;
                            int operadora = int.Parse(registro.Element("Operadora")?.Value ?? "0");
                            string modalidade = registro.Element("Modalidade")?.Value;

                            // Vendas POS (NSU 0000000) sempre são incluídas
                            if (meioCaptura == "1")
                            {
                                bool vendaExiste = context.VendasConciliadas.Any(v =>
                                    v.ValorLiquidoParcela == valorVendaLiquidaParcela &&
                                    v.IdentificadorConciliadora == cliente.IdentificadorConciliadora &&
                                    v.NSU == nSU
                                );

                                if (vendaExiste)
                                    continue;
                            }

                            var venda = new VendasConciliadas(
                                cliente.IdentificadorConciliadora,
                                dataInicial,
                                dataFinal,
                                versao,
                                lote,
                                nomeSistema,
                                produto,
                                descricaoTipoProduto,
                                codigoAutorizacao,
                                identificadorPagamento,
                                dataVenda,
                                dataVencimento,
                                valorVendaParcela,
                                valorVendaLiquidaParcela,
                                valorTotalVendaParcela,
                                taxa,
                                parcela,
                                totalParcelas,
                                valorBrutoMoeda,
                                valorLiquidoMoeda,
                                cotacaoMoeda,
                                moeda,
                                nSU,
                                tID,
                                terminal,
                                meioCaptura,
                                operadora,
                                modalidade,
                                "Não Conciliada",
                                User.Identity.Name
                            );

                            vendasConciliadasList.Add(venda);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[WARNING] Erro ao processar registro: {ex.Message}");
                            // Continue processando os outros registros
                        }
                    }

                    if (!vendasConciliadasList.Any())
                    {
                        return BadRequest("Nenhum registro válido encontrado no XML.");
                    }

                    // Adicionar todas as vendas ao contexto
                    context.VendasConciliadas.AddRange(vendasConciliadasList);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Erro ao processar XML: {ex.Message}");
                }

                // Continuar com o envio para a conciliadora
                var (success, errorMessage, data) = await _conciliadoraService.EnviarVendaSistemaAsync(
                    cliente.SenhaConciliadora,
                    cliente.IdentificadorConciliadora,
                    file);

                VendaSistema vendaSistema;

                if (data.Codigo == 0)
                {
                    var salvarArquivoBlob = await blobStorageService.UploadAsync(file);

                    vendaSistema = new VendaSistema(
                        DateTime.Now,
                        cliente,
                        erps,
                        conteudoXml,
                        salvarArquivoBlob,
                        User.Identity?.Name
                    );

                    vendaSistema.SetDadosRespostaEnvio(
                        data.Codigo,
                        data.Mensagem,
                        null,
                        true
                    );
                }
                else if (data.Codigo == 3)
                {
                    return BadRequest("Senha inválida!");
                }
                else
                {
                    var salvarArquivoBlob = await blobStorageService.UploadAsync(file);

                    vendaSistema = new VendaSistema(
                        DateTime.Now,
                        cliente,
                        erps,
                        conteudoXml,
                        salvarArquivoBlob,
                        User.Identity?.Name
                    );

                    vendaSistema.SetDadosRespostaEnvio(
                        data.Codigo,
                        data.Mensagem,
                        data.XmlErros[0].Erro,
                        false
                    );

                    vendaSistema.Situacao = "Erro";
                }

                context.VendaSistema.Add(vendaSistema);
                context.SaveChanges(); // Salva VendaSistema e VendasConciliadas

                if (data.Codigo == 0)
                {
                    return Ok(new
                    {
                        sucesso = true,
                        codigo = data.Codigo,
                        mensagem = data.Mensagem,
                        idVendaSistema = vendaSistema.IdVendaSistema,
                        xmlTamanho = conteudoXml.Length,
                        vendasProcessadas = vendasConciliadasList.Count
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        sucesso = false,
                        codigo = data.Codigo,
                        mensagem = data.Mensagem,
                        xmlErros = data.XmlErros[0].Erro,
                        idVendaSistema = vendaSistema.IdVendaSistema,
                        xmlTamanho = conteudoXml.Length
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Erro no método Salvar: {ex.Message}");
                Console.WriteLine($"[ERROR] StackTrace: {ex.StackTrace}");

                return StatusCode(500, new
                {
                    sucesso = false,
                    mensagem = $"Erro interno: {ex.Message}"
                });
            }
        }

        [HttpPost]
        [Route("envio-vendas-cartao-credito-json")]
        [AllowAnonymous]
        public async Task<IActionResult> EnvioVendaCartaoCreditoJson([FromBody] VendasConciliadasRequestModelJson model)
        {
                var venda = new VendasConciliadas(
                    identificaodorConciliadora:   model.IdentificadorConciliadora,
                    dataInicial: model.DataInicial,
                    dataFinal:  model.DataFinal,
                    versao: model.Versao,
                    lote: model.Lote,
                    nomeSistema: model.NomeSistema,
                    produto: model.Produto,
                    descricaoTipoProduto: model.DescricaoTipoProduto,
                    codigoAutorizacao: model.CodigoAutorizacao,
                    identificadorPagamento: model.IdentificadorPagamento,
                    dataVenda: model.DataVenda,
                    dataVencimento: model.DataVencimento,
                    valorVendaParcela: model.ValorVendaParcela,
                    valorLiquidoParcela: model.ValorLiquidoParcela,
                    totalVenda: model.TotalVenda,
                    taxa: model.Taxa,
                    parcela: model.Parcela,
                    totalParcelas: model.TotalParcelas,
                    valorBrutoMoeda: model.ValorBrutoMoeda,
                    valorLiquidoMoeda: model.ValorLiquidoMoeda,
                    cotacaoMoeda: model.CotacaoMoeda,
                    moeda: model.Moeda,
                    nSU: model.NSU,
                    tID: model.TID,
                    terminal: model.Terminal,
                    meioCaptura: model.MeioCaptura,
                    operadora: model.Operadora,
                    modalidade: model.Modalidade,
                    status: model.Status,
                    usuarioInclusao: "admin"
                );

              var x = context.VendasConciliadas.Add(venda);
              await context.SaveChangesAsync();
              return Ok();
        }
        
        [HttpPost]
        [Route("listar-arquivos-enviados-erp")]
        public IActionResult Listar([FromBody] FiltroArquivosEnviadosModel model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name && x.TipoUsuario == "ERP");
            if (usuarioLogado == null)
            {
                return BadRequest("Usuário não encontrado.");
            }
             

            var result = context.VendaSistema.Include(c => c.Cliente.Pessoa)
                .Include(c => c.ERPs)
                .Where(x => x.IdERPs == usuarioLogado.IdERPs &&
                   x.Data.Date >= model.DataInicio.Date &&
                   x.Data.Date <= model.DataTermino.Date)
                .Select(c => new
                {
                    c.IdVendaSistema,
                    c.Data,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdERPs,
                    NomeERP = c.ERPs.Nome,
                    c.XMLErrosResposta,
                    c.Arquivo,
                    c.Situacao
                }).AsQueryable();

            if (model.IdCliente > 0)
            {
                result = result.Where(x=>x.IdCliente == model.IdCliente);
            }
            var lista = result.OrderByDescending(x=>x.IdVendaSistema).ToList();
            return Ok(lista);
        } 

        [HttpGet]
        [Route("listar-clientes")]
        public async Task<IActionResult> ListarClientes()
        {

            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name && x.TipoUsuario == "ERP");
            if (usuarioLogado == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            var result = await context.VendaSistema.
                Include(c => c.Cliente.Pessoa)
                .Include(c => c.ERPs)
                .Where(x => x.IdERPs == usuarioLogado.IdERPs)
                .Select(c => new
                {
                    c.IdVendaSistema,
                    c.Data,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdERPs,
                    NomeERP = c.ERPs.Nome,
                    c.Arquivo,
                    c.Situacao
                }).ToListAsync();
            return Ok(result);
        }


        [HttpPost]
        [Route("envio-movimentacao-diaria")]
        public async Task<IActionResult> EnvioVendaMovimentacaoDiaria([FromBody] MovimentacaoDiariaRequestModel request)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name && x.TipoUsuario == "ERP");
            if (usuarioLogado == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            var cliente = context.Cliente.Include(x => x.ERPs).FirstOrDefault(x => x.IdentificadorConciliadora == request.IdCliente.ToString());
            if (cliente == null)
            {
                return BadRequest("Cliente não encontrado.");
            }

            var erps = context.ERPs.FirstOrDefault(x => x.IdERPs == usuarioLogado.IdERPs);
            if (erps == null)
            {
                return BadRequest("ERP não encontrado.");
            }
             

            return Ok();
        }

    }
}


