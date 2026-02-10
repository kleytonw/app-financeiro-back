using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using ERP.Models;
using ERP_API.Domain.Entidades;
using System.Data.Entity;
using System.Collections.Generic;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class DiagnosticoController : ControllerBase
    {
        protected Context context;
        public DiagnosticoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Diagnostico
                  .Select(m => new
                  {
                      m.IdDiagnostico,
                      m.Data,
                      m.Unidade.Nome,
                      m.QtdeTransacoes,
                      m.QtdeVendas,
                      m.QtdeTransacoesConciliadas,
                      m.QtdeTransacoesInconsistentes,
                      m.QtdeTransacoesNaoEncontradas,
                      m.QtdeVendasConciliadas,
                      m.QtdeVendasInconsistentes,
                      m.QtdeVendasNaoEncontradas,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarUnidade")]
        public IActionResult ListarUnidade()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.Unidade.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                  .Select(m => new
                  {
                      m.IdUnidade,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarDiagnosticoRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var query = context.Diagnostico.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa && x.Data >= model.DataInicio && x.Data <= model.DataTermino).AsQueryable();

            if (model.IdUnidade != 0) 
            {
                query = query.Where(x => x.IdUnidade == model.IdUnidade);
            }


            var result = query.Select(m => new
            {
                m.IdDiagnostico,
                m.Data,
                m.Unidade.Nome,
                m.IdUnidade,
                m.QtdeTransacoes,
                m.QtdeVendas,
                m.QtdeTransacoesConciliadas,
                m.QtdeTransacoesInconsistentes,
                m.QtdeTransacoesNaoEncontradas,
                m.QtdeVendasConciliadas,
                m.QtdeVendasInconsistentes,
                m.QtdeVendasNaoEncontradas,
                m.QtdePagamentosEncontrados,
                m.QtdePagamentosNaoEncontrados,
                m.StatusDiagnostico,
                m.Situacao
            }).Take(1000).ToList();

            var totalTransacoes = query.Sum(x => x.QtdeTransacoes ?? 0);
            var totalVendas = query.Sum(x => x.QtdeVendas ?? 0);
            var totalTransacoesConciliadas = query.Sum(x => x.QtdeTransacoesConciliadas ?? 0);
            var totalTransacoesInconsistentes = query.Sum(x => x.QtdeTransacoesInconsistentes ?? 0);
            var totalTransacoesNaoEncontradas = query.Sum(x => x.QtdeTransacoesNaoEncontradas ?? 0);
            var totalVendasConciliadas = query.Sum(x => x.QtdeVendasConciliadas ?? 0);
            var totalVendasInconsistentes = query.Sum(x => x.QtdeVendasInconsistentes ?? 0);
            var totalVendasNaoEncontradas = query.Sum(x => x.QtdeVendasNaoEncontradas ?? 0);
            var totalPagamentosEncontrados = query.Sum(x => x.QtdePagamentosEncontrados ?? 0);
            var totalPagamentosNaoEncontrados = query.Sum(x => x.QtdePagamentosNaoEncontrados ?? 0);


            var totais = new
            {
                TotalTransacoes = totalTransacoes,
                TotalVendas = totalVendas,
                TotalTransacoesConciliadas = totalTransacoesConciliadas,
                TotalTransacoesInconsistentes = totalTransacoesInconsistentes,
                TotalTransacoesNaoEncontradas = totalTransacoesNaoEncontradas,
                TotalVendasConciliadas = totalVendasConciliadas,
                TotalVendasInconsistentes = totalVendasInconsistentes,
                TotalVendasNaoEncontradas = totalVendasNaoEncontradas,
                TotalPagamentosNaoEncontrados = totalPagamentosNaoEncontrados,
                TotalPagamentosEncontrados = totalPagamentosEncontrados
            };

            return Ok(new
            {
                result,
                totais
            });
        }

        [HttpPost]
        [Route("pesquisarTransacoesNaoEncontradas")]
        public IActionResult PesquisarTransacoesNaoEncotradas([FromBody] PesquisarDiagnosticoRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var query = context.Transacao.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa && x.DataVenda == model.Data && x.StatusConciliacao == "Não encontrada" ).AsQueryable();

            if (model.IdUnidade != 0)
            {
                query = query.Where(x => x.IdUnidade == model.IdUnidade);
            }


            var result = query.Select(m => new
            {
                m.IdTransacao,
                m.Cliente,
                m.DataVenda,
                m.Unidade.Nome,
                m.ValorBruto,
                m.Taxa,
                m.Despesa,
                m.ValorLiquido,
                m.DataMovimentacao,
                m.MeioPagamento,
                m.Bandeira,
                m.Identificador,
                m.Situacao
            }).Take(1000).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisarVendasNaoEncontradas")]
        public IActionResult PesquisarVendasNaoEncotradas([FromBody] PesquisarDiagnosticoRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var query = context.Venda.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa && x.DataVenda == model.Data && x.StatusConciliacao == "Não encontrada").AsQueryable();

            if (model.IdUnidade != 0)
            {
                query = query.Where(x => x.IdUnidade == model.IdUnidade);
            }


            var result = query.Select(m => new
            {
                m.IdVenda,
                m.Cliente,
                m.DataVenda,
                m.Unidade.Nome,
                m.ValorBruto,
                m.MeioPagamento,
                m.Bandeira.NomeBandeira,
                m.Operadora.NomeOperadora,
                m.Identificador,
                m.Situacao
            }).Take(1000).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisarPagamentosNaoEncontrados")]
        public IActionResult PesquisarPagamentosNaoEncotrados([FromBody] PesquisarDiagnosticoRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var query = context.Pagamento.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa && x.DataPagamento == model.Data && x.StatusConciliado == false).AsQueryable();

            if (model.IdUnidade != 0)
            {
                query = query.Where(x => x.IdUnidade == model.IdUnidade);
            }


            var result = query.Select(m => new
            {
                m.IdPagamento,
                m.DataPagamento,
                m.NomeBanco,
                m.Agencia,
                m.Conta,
                m.NomeBandeira,
                m.RazaoSocial,
                m.ValorPagamento,
                m.StatusPagamento,
                m.TipoPagamento,
                m.Situacao
            }).Take(1000).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisarPagamentosEncontrados")]
        public IActionResult PesquisarPagamentosEncotrados([FromBody] PesquisarDiagnosticoRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var query = context.Pagamento.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa && x.DataPagamento == model.Data && x.StatusConciliado == true).AsQueryable();

            if (model.IdUnidade != 0)
            {
                query = query.Where(x => x.IdUnidade == model.IdUnidade);
            }


            var result = query.Select(m => new
            {
                m.IdPagamento,
                m.DataPagamento,
                m.NomeBanco,
                m.Agencia,
                m.Conta,
                m.NomeBandeira,
                m.RazaoSocial,
                m.ValorPagamento,
                m.StatusPagamento,
                m.TipoPagamento,
                m.Situacao
            }).Take(1000).ToList();

            return Ok(result);
        }

        /*[HttpPost]
        [Route("conciliar")]
        public IActionResult ConciliarBancaria([FromBody] DiagnosticoRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);

            var qtdeVendas = 0;
            var qtdeConciliada = 0;
            var qtdeInconsistente = 0;
            var qtdeNaoEncontrada = 0;


            // verifica se existe um diagonisto para a unidade e operadora 

            // se exisiter incrementa os valores 

            // senão cria um novo diagonstico e incrementa os valores 

            var transacoes = context.Transacao
                .Where(t => t.DataVenda.Value.Date == model.Data.Date && t.IdUnidade == model.IdUnidade)
                .ToList();


            foreach (var transacao in transacoes)
            {
                var vendasAgrupados = context.Venda
               .Where(r => r.IdUnidade == model.IdUnidade &&
                           r.DataPagamento.Value.Date >= model.Data.Date &&
                           r.MeioPagamento.ToUpper() == transacao.MeioPagamento.ToUpper() &&
                           r.NomeBandeira == transacao.Bandeira)
               .GroupBy(r => r.Identificador)
               .Select(g => new
               {
                   Identificador = g.Key,
                   SomaValorPagamento = g.Sum(r => r.ValorBruto)
               })
               .ToDictionary(g => g.Identificador, g => g.SomaValorPagamento);

                if (vendasAgrupados.TryGetValue(transacao.Identificador, out var somaValorPagamento))
                {
                    qtdeVendas++;

                    if (Math.Abs((transacao.ValorBruto ?? 0) - (somaValorPagamento ?? 0)) <= 0.2m)
                    {
                        transacao.SetConciliacao("Conciliado", "");
                        qtdeConciliada++;
                    }
                    else
                    {
                        transacao.SetConciliacao("Inconsistente", "O valor de venda está diferente do valor da transação! Soma do valor das vendas: " + somaValorPagamento);
                        qtdeInconsistente++;
                    }
                }
                else
                {
                    transacao.SetConciliacao("Não encontrada", "");
                    qtdeNaoEncontrada++;
                }

                context.Update(transacao);
                context.SaveChanges();
            }

            var diagnostico = new Diagnostico(model.Data, transacoes.Count, qtdeVendas, qtdeConciliada, qtdeInconsistente, qtdeNaoEncontrada, empresa, unidade, User.Identity.Name);
            context.Diagnostico.Add(diagnostico);
            context.SaveChanges();

            return Ok(new
            {
                Mensagem = "Conciliação concluída com sucesso!",
                TotalTransacoes = transacoes.Count,
                Conciliados = qtdeConciliada,
                Inconsistentes = qtdeInconsistente,
                NaoEncontrada = qtdeNaoEncontrada
            });
        }*/

        [HttpPost]
        [Route("conciliar2")]
        public IActionResult ConciliarBancari2([FromBody] DiagnosticoRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);

            int qtdeTransacoesConciliadas = 0, 
                qtdeTransacoesInconsistentes = 0, 
                qtdeTransacoesNaoEncontradas = 0, 
                qtdeVendasConciliadas = 0, 
                qtdeVendasNaoEncontradas = 0, 
                qtdeVendasInconsistentes = 0;

            var transacoes = context.Transacao
                .AsNoTracking().Include(x => x.Bandeira)
                .Where(t => t.DataVenda.Value.Date == model.Data.Date && t.IdUnidade == model.IdUnidade)
                .ToList();

            var vendas = context.Venda
                .Where(r => r.IdUnidade == model.IdUnidade &&
                            r.DataVenda.HasValue && r.DataVenda.Value.Date >= model.Data.Date)
                .ToList();

            var vendasAgrupadas = vendas
                 .Where(r => !string.IsNullOrEmpty(r.Identificador))
                 .GroupBy(r => new { r.Identificador, r.MeioPagamento, Bandeira = r.NomeBandeira })
                 .ToDictionary(
                     g => g.Key,
                     g => g.Sum(r => r.ValorBruto)
                 );

                var vendasConciliadas = new HashSet<object>();

            foreach (var transacao in transacoes)
            {
                var chave = new
                {
                    Identificador = string.IsNullOrEmpty(transacao.Identificador) ? "Sem Identificador" : transacao.Identificador,
                    transacao.MeioPagamento,
                    transacao.Bandeira
                };

                if (vendasAgrupadas.TryGetValue(chave, out var somaValorPagamento))
                {
                    if (Math.Abs((transacao.ValorBruto ?? 0) - (somaValorPagamento ?? 0)) <= 0.2m)
                    {
                        transacao.SetConciliacao("Conciliado", "");
                        qtdeTransacoesConciliadas++;
                        vendasConciliadas.Add(chave);
                    }
                    else
                    {
                        transacao.SetConciliacao("Inconsistente", $"O valor de venda está diferente! Soma do valor das vendas: {somaValorPagamento}");
                        qtdeTransacoesInconsistentes++;
                    }
                }
                else
                {
                    transacao.SetConciliacao("Não encontrada", "");
                    qtdeTransacoesNaoEncontradas++;
                }
            }

            foreach (var venda in vendas)
            {
                var chaveVenda = new
                {
                    Identificador = string.IsNullOrEmpty(venda.Identificador) ? "Sem Identificador" : venda.Identificador,
                    venda.MeioPagamento,
                    Bandeira = venda.NomeBandeira
                };
                if (vendasAgrupadas.ContainsKey(chaveVenda))
                {
                    var somaValorTransacao = transacoes
                        .Where(t => t.Identificador == venda.Identificador
                                 && t.MeioPagamento == venda.MeioPagamento
                                 && t.Bandeira == venda.NomeBandeira)
                        .Sum(t => t.ValorBruto ?? 0);

                    if (somaValorTransacao > 0)
                    {
                        if (Math.Abs((venda.ValorBruto ?? 0) - (somaValorTransacao)) <= 0.2m)
                        {
                            venda.SetConciliacao("Conciliado", "");
                            qtdeVendasConciliadas++;
                        }
                        else
                        {
                            venda.SetConciliacao("Inconsistente", $"O valor da transação ({somaValorTransacao}) não bate com o valor da venda ({venda.ValorBruto})!");
                            qtdeVendasInconsistentes++;
                        }
                    }
                    else
                    {
                        venda.SetConciliacao("Não encontrada", "Nenhuma transação correspondente foi encontrada para essa venda!");
                        qtdeVendasNaoEncontradas++;
                    }
                }
            }
                context.UpdateRange(transacoes);
                context.UpdateRange(vendas);

                var diagnostico = new Diagnostico(model.Data, transacoes.Count, vendasAgrupadas.Count, qtdeTransacoesConciliadas, qtdeTransacoesInconsistentes, qtdeTransacoesNaoEncontradas, qtdeVendasConciliadas, qtdeVendasInconsistentes, qtdeVendasNaoEncontradas, empresa, unidade, User.Identity.Name);
                context.Diagnostico.Add(diagnostico);
                context.SaveChanges();

                var qtdePagamentosNaoEncontrados = 0;
                var qtdePagamentosEncontrados = 0;

            foreach (var adquirente in context.Operadora.ToList())
                {
                    var pagamentos = context.Pagamento
                    .Where(r => r.IdUnidade == model.IdUnidade && r.DataPagamento.Value.Date == model.Data.Date && r.IdOperadora == adquirente.IdOperadora)
                    .ToList();

                foreach (var pagamento in pagamentos)
                    {
                        var extrato = context.Extrato
                            .FirstOrDefault(r => r.IdCliente== model.IdCliente
                                            && r.DataLancamento.Date == model.Data.Date
                                            && r.Valor == pagamento.ValorPagamento);
                        if (extrato != null)
                        {
                            pagamento.Conciliar(true);
                            qtdePagamentosEncontrados++;
                            pagamento.SetIdDiagnostico(diagnostico.IdDiagnostico);
                            context.Pagamento.Update(pagamento);
                            context.SaveChanges();
                        }
                        else
                        {
                        pagamento.Conciliar(false);
                        qtdePagamentosNaoEncontrados++;
                        pagamento.SetIdDiagnostico(diagnostico.IdDiagnostico);
                        context.Pagamento.Update(pagamento);
                        context.SaveChanges();
                         }
                    }
                }

                diagnostico.SetPagamentosEncontradosENaoEncontrados(qtdePagamentosEncontrados, qtdePagamentosNaoEncontrados, User.Identity.Name);

                if (diagnostico.QtdeVendasInconsistentes == 0
                    && diagnostico.QtdeVendasNaoEncontradas == 0
                    && diagnostico.QtdeTransacoesInconsistentes == 0
                    && diagnostico.QtdeTransacoesNaoEncontradas == 0
                    && diagnostico.QtdePagamentosNaoEncontrados == 0)
                {
                    diagnostico.AlterarStatus("Aprovado", User.Identity.Name);
                }
                else
                {
                    diagnostico.AlterarStatus("Inconsistente", User.Identity.Name);
                }

                context.Update(diagnostico);
                context.SaveChanges();

                return Ok(new
                    {
                        Mensagem = "Conciliação concluída!",
                        TotalTransacoes = transacoes.Count,
                        TotalVendas = vendas.Count,
                        Conciliados = qtdeTransacoesConciliadas,
                        Inconsistentes = qtdeTransacoesInconsistentes,
                        NaoEncontrada = qtdeTransacoesNaoEncontradas,
                        VendasNaoEncontradas = qtdeVendasNaoEncontradas,
                        VendasInconsistentes = qtdeVendasInconsistentes,
                        VendasConciliadas = qtdeVendasConciliadas,
                        PagamentosEncontrados = qtdePagamentosEncontrados,
                        PagamentosNaoEncontrados = qtdePagamentosNaoEncontrados,
                });
            
            }




        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var diagnostico = context.Diagnostico.FirstOrDefault(x => x.IdDiagnostico == id && x.IdEmpresa == usuarioLogado.IdEmpresa);
            diagnostico.Excluir(User.Identity.Name);

            context.Update(diagnostico);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var diagnostico = context.Diagnostico.FirstOrDefault(x => x.IdDiagnostico == id && x.IdEmpresa == usuarioLogado.IdEmpresa);
            if (diagnostico == null)
                return BadRequest("Transação não encontrado ");

            return Ok(new DiagnosticoResponse()
            {
                IdDiagnostico = diagnostico.IdDiagnostico,
                QtdeTransacoes = diagnostico.QtdeTransacoes,
                QtdeVendas = diagnostico.QtdeVendas,
                QtdeTransacoesConciliadas = diagnostico.QtdeTransacoesConciliadas,
                QtdeTransacoesInconsistentes = diagnostico.QtdeTransacoesInconsistentes,
                QtdeTransacoesNaoEncontradas = diagnostico.QtdeTransacoesNaoEncontradas,
                Situacao = diagnostico.Situacao
            });
        }
    }
}


