using Microsoft.EntityFrameworkCore;
using ERP.Infrastructure.Mapping;
using ERP.Models;
using ERP.Domain;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using ERP_API.Infrastructure.Mapping;
using System.Collections.Generic;
using ERP_API.Domain;

namespace ERP.Infra
{
    public class Context : DbContext
    {

        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<GrupoProduto> GrupoProduto { get; set; }
        public DbSet<Regiao> Regiao { get; set; }
        public DbSet<Setor> Setor { get; set; }
        public DbSet<RamoAtividade> RamoAtividade { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<GrupoEmpresa> GrupoEmpresa { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Plano> Plano { get; set; }
        public DbSet<GrupoComissao> GrupoComissao { get; set; }
        public DbSet<Tributacao> Tributacao { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Servico> Servico { get; set; }
        public DbSet<Movimentacao> Movimentacao { get; set; }
        public DbSet<MovimentacaoItem> MovimentacaoItem { get; set; }
        public DbSet<MovimentacaoFatura> MovimentacaoFatura { get; set; }
        public DbSet<MovimentacaoDuplicata> MovimentacaoDuplicata { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Afiliado> Afiliado { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Parceiro> Parceiro { get; set; }
        public DbSet<Adquirente> Adquirente { get; set; }
        public DbSet<SetorProduto> SetorProduto { get; set; }
        public DbSet<MovimentacaoLog> MovimentacaoLog { get; set; }
        // public DbSet<MovimentacaoFatura> MovimentacaoFatura { get; set; }
        public DbSet<Consultor> Consultor { get; set; }
        public DbSet<Provedor> Provedor { get; set; }
        public DbSet<TipoMensagem> TipoMensagem { get; set; }
        public DbSet<Mensagem> Mensagem { get; set; }
        public DbSet<MensagemLog> MensagemLog { get; set; }
        public DbSet<RegiaoConsultor> RegiaoConsultor { get; set; }
        public DbSet<TipoSuporte> TipoSuporte { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Disciplina> Disciplina { get; set; }
        public DbSet<DisciplinaCurso> DisciplinaCurso { get; set; }
        public DbSet<Localizacao> Localizacao { get; set; }
        public DbSet<Caracteristica> Caracteristica { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacaoEstoque { get; set; }
        public DbSet<UnidadeMedida> UnidadeMedida { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<BlogComentario> BlogComentario { get; set; }
        public DbSet<Newsletter> Newsletter { get; set; }
        public DbSet<ContatoSite> ContatoSite { get; set; }
        public DbSet<Contrato> Contrato { get; set; }
        public DbSet<Vendedor> Vendedor { get; set; }
        public DbSet<Carrinho> Carrinho { get; set; }
        public DbSet<LancamentoItem> LancamentoItem { get; set; }
        public DbSet<TabelaPreco> TabelaPreco { get; set; }
        public DbSet<TabelaPrecoItem> TabelaPrecoItem { get; set; }
        public DbSet<Transacao> Transacao { get; set; }
        public DbSet<Bandeira> Bandeira { get; set; }
        public DbSet<Unidade> Unidade { get; set; }
        public DbSet<Venda> Venda { get; set; }
        public DbSet<Diagnostico> Diagnostico { get; set; }
        public DbSet<MeioPagamento> MeioPagamento { get; set; }
        public DbSet<Operadora> Operadora { get; set; }
        public DbSet<ContratoOperadora> ContratoOperadora { get; set; }
        public DbSet<ContratoOperadoraTaxa> ContratoOperadoraTaxa { get; set; }
        public DbSet<UnidadeParametro> UnidadeParametro { get; set; }
        public DbSet<ContaBancaria> ContaBancaria { get; set; }
        public DbSet<Banco> Banco { get; set; }
        public DbSet<Extrato> Extrato { get; set; }
        public DbSet<ParceiroParametro> ParceiroParametro { get; set; }
        public DbSet<ParceiroSistema> ParceiroSistema { get; set; }
        public DbSet<LogWebhookExtratoTecnospeed> LogWebhookExtratoTecnopeed { get; set; }
        public DbSet<Pagamento> Pagamento { get; set; }
        public DbSet<ClienteContato> ClienteContato { get; set; }
        public DbSet<BIFaturamentoPeriodoReais> BIFaturamentoPeriodoReais { get; set; }
        public DbSet<ClasseTarifa> ClasseTarifa { get; set; }
        public DbSet<ClasseTarifaItem> ClasseTarifaItem { get; set; }
        public DbSet<ClasseRecebimento> ClasseRecebimento { get; set; }
        public DbSet<ClasseRecebimentoItem> ClasseRecebimentoItem { get; set; }
        public DbSet<ContratoOperadoraRecebimento> ContratoOperadoraRecebimento { get; set; }
        public DbSet<BIFaturamentoPeriodoPorcentagem> BIFaturamentoPeriodoPorcentagem { get; set; }
        public DbSet<BILucroBrutoPeriodoReais> BILucroBrutoPeriodoReais { get; set; }
        public DbSet<BILucroBrutoPeriodoPorcentagem> BILucroBrutoPeriodoPorcentagem { get; set; }
        public DbSet<ClasseAntecipacao> ClasseAntecipacao { get; set; }
        public DbSet<ClasseAntecipacaoItem> ClasseAntecipacaoItem { get; set; }
        public DbSet<ContratoOperadoraAntecipacao> ContratoOperadoraAntecipacao { get; set; }
        public DbSet<BINumeroClientes> BINumeroClientes { get; set; }
        public DbSet<BITicketMedio> BITicketMedio { get; set; }
        public DbSet<BITicketPorSetor> BITicketPorSetor { get; set; }
        public DbSet<BITicketMedioPorSetor> BITicketMedioPorSetor { get; set; }
        public DbSet<BIMargemPorcentagem> BIMargemPorcentagem { get; set; }
        public DbSet<Financeiro> Financeiro { get; set; }
        public DbSet<FinanceiroParcela> FinanceiroParcela { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<TipoDocumento> TipoDocumento { get; set; }
        public DbSet<ClienteDocumento> ClienteDocumento { get; set; }
        public DbSet<ERPs> ERPs { get; set; }
        public DbSet<ClienteERP> ClienteERP { get; set; }
        public DbSet<ClienteAdquirente> ClienteAdquirente { get; set; }
        public DbSet<ClienteParametros> ClienteParametros { get; set; }
        public DbSet<VendaSistema> VendaSistema { get; set; }
        public DbSet<VendaNfe> VendaNfe { get; set; }
        public DbSet<Proposta> Proposta { get; set; }
        public DbSet<UsuarioCliente> UsuarioCliente {  get; set; }
        public DbSet<Etapa> Etapa { get; set; }
        public DbSet<ControleCartaVan> ControleCartaVan { get; set; }
        public DbSet<ControleCartaVanHistorico> ControleCartaVanHistorico { get; set; }
        public DbSet<ClienteContaBancaria> ClienteContaBancaria { get; set; }
        public DbSet<ConciliacaoBancaria> ConciliacaoBancaria { get; set; }
        public DbSet<WebhookPluggy> WebHookPluggy { get; set; }
        public DbSet<MovimentacaoDiaria> MovimentacaoDiaria { get; set; }
        public DbSet<VendasConciliadas> VendasConciliadas { get; set; }
        public DbSet<VendasConciliadas_bkp> VendasConciliadas_bkp { get; set; }
        public DbSet<PlanoConta> PlanoConta { get; set; }
        public DbSet<Colaborador> Colaborador { get; set; }
        public DbSet<ClienteBanco> ClienteBanco { get; set; }
        public DbSet<TicketMensagem> TicketMensagem { get; set; }
        public DbSet<PlanoComissao> PlanoComissao { get; set; }
        public DbSet<ConectorOpenFinance> ConectorOpenFinance { get; set; }
        public DbSet<Faturamento> Faturamento {  get; set; }
        public DbSet<ConfiguracaoConciliacao> ConfiguracaoConciliacao { get; set; }
        public DbSet<ServicoNfse> ServicoNfse { get; set; }
        public DbSet<Nfse> Nfse { get; set; }
        public DbSet<ConfiguracaoNfse> ConfiguracaoNfse { get; set; }

        public Context (DbContextOptions options) : base(options)
        {
        }   
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(new PessoaMap().Configure);
            modelBuilder.Entity<Usuario>(new UsuarioMap().Configure);
            modelBuilder.Entity<GrupoProduto>(new GrupoProdutoMap().Configure);
            modelBuilder.Entity<Regiao>(new RegiaoMap().Configure);
            modelBuilder.Entity<Setor>(new SetorMap().Configure);
            modelBuilder.Entity<RamoAtividade>(new RamoAtividadeMap().Configure);
            modelBuilder.Entity<Produto>(new ProdutoMap().Configure);
            modelBuilder.Entity<GrupoEmpresa>(new GrupoEmpresaMap().Configure);
            modelBuilder.Entity<Empresa>(new EmpresaMap().Configure);
            modelBuilder.Entity<Movimentacao>(new MovimentacaoMap().Configure);
            modelBuilder.Entity<MovimentacaoItem>(new MovimentacaoItemMap().Configure);
            modelBuilder.Entity<MovimentacaoFatura>(new MovimentacaoFaturaMap().Configure);
            modelBuilder.Entity<Plano>(new PlanoMap().Configure);
            modelBuilder.Entity<GrupoComissao>(new GrupoComissaoMap().Configure);
            modelBuilder.Entity<Tributacao>(new TributacaoMap().Configure);
            modelBuilder.Entity<Curso>(new CursoMap().Configure);
            modelBuilder.Entity<Servico>(new ServicoMap().Configure);
            modelBuilder.Entity<Fornecedor>(new FornecedorMap().Configure);
            modelBuilder.Entity<Afiliado>(new AfiliadoMap().Configure);
            modelBuilder.Entity<Cliente>(new ClienteMap().Configure);
            modelBuilder.Entity<Parceiro>(new ParceiroMap().Configure);
            modelBuilder.Entity<Adquirente>(new AdquirenteMap().Configure);
            modelBuilder.Entity<SetorProduto>(new SetorProdutoMap().Configure);
            modelBuilder.Entity<MovimentacaoLog>(new MovimentacaoLogMap().Configure);
            modelBuilder.Entity<Consultor>(new ConsultorMap().Configure);
            modelBuilder.Entity<Provedor>(new ProvedorMap().Configure);
            modelBuilder.Entity<TipoMensagem>(new TipoMensagemMap().Configure);
            modelBuilder.Entity<Mensagem>(new MensagemMap().Configure);
            modelBuilder.Entity<MensagemLog>(new MensagemLogMap().Configure);
            modelBuilder.Entity<RegiaoConsultor>(new RegiaoConsultorMap().Configure);
            modelBuilder.Entity<TipoSuporte>(new TipoSuporteMap().Configure);
            modelBuilder.Entity<Ticket>(new TicketMap().Configure);
            modelBuilder.Entity<Disciplina>(new DisciplinaMap().Configure);
            modelBuilder.Entity<DisciplinaCurso>(new DisciplinaCursoMap().Configure);
            // modelBuilder.Entity<MovimentacaoFatura>(new MovimentacaoFaturaMap().Configure);
            modelBuilder.Entity<MovimentacaoDuplicata>(new MovimentacaoDuplicataMap().Configure);
            modelBuilder.Entity<Localizacao>(new LocalizacaoMap().Configure);
            modelBuilder.Entity<Caracteristica>(new CaracteristicaMap().Configure);
            modelBuilder.Entity<MovimentacaoEstoque>(new MovimentacaoEstoqueMap().Configure);
            modelBuilder.Entity<UnidadeMedida>(new UnidadeMedidaMap().Configure);
            modelBuilder.Entity<Blog>(new BlogMap().Configure);
            modelBuilder.Entity<BlogComentario>(new BlogComentarioMap().Configure);
            modelBuilder.Entity<Newsletter>(new NewsletterMap().Configure);
            modelBuilder.Entity<ContatoSite>(new ContatoSiteMap().Configure);
            modelBuilder.Entity<Contrato>(new ContratoMap().Configure);
            modelBuilder.Entity<Vendedor>(new VendedorMap().Configure);
            modelBuilder.Entity<Carrinho>(new CarrinhoMap().Configure);
            modelBuilder.Entity<LancamentoItem>(new LancamentoItemMap().Configure);
            modelBuilder.Entity<TabelaPreco>(new TabelaPrecoMap().Configure);
            modelBuilder.Entity<TabelaPrecoItem>(new TabelaPrecoItemMap().Configure);
            modelBuilder.Entity<Transacao>(new TransacaoMap().Configure);
            modelBuilder.Entity<Bandeira>(new BandeiraMap().Configure);
            modelBuilder.Entity<Unidade>(new UnidadeMap().Configure);
            modelBuilder.Entity<Venda>(new VendaMap().Configure);
            modelBuilder.Entity<Diagnostico>(new DiagnosticoMap().Configure);
            modelBuilder.Entity<MeioPagamento>(new MeioPagamentoMap().Configure);
            modelBuilder.Entity<Operadora>(new OperadoraMap().Configure);
            modelBuilder.Entity<ContratoOperadora>(new ContratoOperadoraMap().Configure);
            modelBuilder.Entity<ContratoOperadoraTaxa>(new ContratoOperadoraTaxaMap().Configure);
            modelBuilder.Entity<UnidadeParametro>(new UnidadeParametroMap().Configure);
            modelBuilder.Entity<ContaBancaria>(new ContaBancariaMap().Configure);
            modelBuilder.Entity<Banco>(new BancoMap().Configure);
            modelBuilder.Entity<Extrato>(new ExtratoMap().Configure);
            modelBuilder.Entity<ParceiroParametro>(new ParceiroParametroMap().Configure);
            modelBuilder.Entity<ParceiroSistema>(new ParceiroSistemaMap().Configure);
            modelBuilder.Entity<LogWebhookExtratoTecnospeed>(new LogWebhookExtratoTecnospeedMap().Configure);
            modelBuilder.Entity<Pagamento>(new PagamentoMap().Configure);
            modelBuilder.Entity<ClienteContato>(new ClienteContatoMap().Configure);
            modelBuilder.Entity<BIFaturamentoPeriodoReais>(new BIFaturamentoPeriodoReaisMap().Configure);
            modelBuilder.Entity<ClasseTarifa>(new ClasseTarifaMap().Configure);
            modelBuilder.Entity<ClasseTarifaItem>(new ClasseTarifaItemMap().Configure);
            modelBuilder.Entity<ClasseRecebimento>(new ClasseRecebimentoMap().Configure);
            modelBuilder.Entity<ClasseRecebimentoItem>(new ClasseRecebimentoItemMap().Configure);
            modelBuilder.Entity<ContratoOperadoraRecebimento>(new ContratoOperadoraRecebimentoMap().Configure);
            modelBuilder.Entity<BIFaturamentoPeriodoPorcentagem>(new BIFaturamentoPeriodoPorcentagemMap().Configure);
            modelBuilder.Entity<BILucroBrutoPeriodoReais>(new BILucroBrutoPeriodoReaisMap().Configure);
            modelBuilder.Entity<BILucroBrutoPeriodoPorcentagem>(new BILucroBrutoPeriodoPorcentagemMap().Configure);
            modelBuilder.Entity<ClasseAntecipacao>(new ClasseAntecipacaoMap().Configure);
            modelBuilder.Entity<ClasseAntecipacaoItem>(new ClasseAntecipacaoItemMap().Configure);
            modelBuilder.Entity<ContratoOperadoraAntecipacao>(new ContratoOperadoraAntecipacaoMap().Configure);
            modelBuilder.Entity<BINumeroClientes>(new BINumeroClientesMap().Configure);
            modelBuilder.Entity<BITicketMedio>(new BITicketMedioMap().Configure);
            modelBuilder.Entity<BITicketPorSetor>(new BITicketPorSetorMap().Configure);
            modelBuilder.Entity<BITicketMedioPorSetor>(new BITicketMedioPorSetorMap().Configure);
            modelBuilder.Entity<BIMargemPorcentagem>(new BIMargemPorcentagemMap().Configure);
            modelBuilder.Entity<Financeiro>(new FinanceiroMap().Configure);
            modelBuilder.Entity<FinanceiroParcela>(new FinanceiroParcelaMap().Configure);
            modelBuilder.Entity<Categoria>(new CategoriaMap().Configure);
            modelBuilder.Entity<TipoDocumento>( new TipoDocumentoMap().Configure);
            modelBuilder.Entity<ClienteDocumento>(new ClienteDocumentoMap().Configure);
            modelBuilder.Entity<ERPs>(new ERPsMap().Configure);
            modelBuilder.Entity<ClienteERP>(new ClienteERPMap().Configure);
            modelBuilder.Entity<ClienteAdquirente>(new ClienteAdquirenteMap().Configure);
            modelBuilder.Entity<ClienteParametros>(new ClienteParametrosMap().Configure);
            modelBuilder.Entity<VendaSistema>(new VendaSistemaMap().Configure);
            modelBuilder.Entity<VendaNfe>(new VendaNfeMap().Configure);
            modelBuilder.Entity<Proposta>(new PropostaMap().Configure);
            modelBuilder.Entity<UsuarioCliente>(new UsuarioClienteMap().Configure);
            modelBuilder.Entity<Etapa>(new EtapaMap().Configure);
            modelBuilder.Entity<ControleCartaVan>(new ControleCartaVanMap().Configure);
            modelBuilder.Entity<ClienteContaBancaria>(new ClienteContaBancariaMap().Configure);
            modelBuilder.Entity<ControleCartaVanHistorico>(new ControleCartaVanHistoricoMap().Configure);
            modelBuilder.Entity<ConciliacaoBancaria>(new ConciliacaoBancariaMap().Configure);
            modelBuilder.Entity<WebhookPluggy>(new WebhookPluggyMap().Configure);
            modelBuilder.Entity<MovimentacaoDiaria>(new MovimentacaoDiariaMap().Configure);
            modelBuilder.Entity<VendasConciliadas>(new VendasConcilidasMap().Configure);
            modelBuilder.Entity<VendasConciliadas_bkp>(new VendasConciliadas_bkpMap().Configure);
            modelBuilder.Entity<PlanoConta>(new PlanoContaMap().Configure);
            modelBuilder.Entity<Colaborador>(new ColaboradorMap().Configure);
            modelBuilder.Entity<ClienteBanco>(new ClienteBancoMap().Configure);
            modelBuilder.Entity<TicketMensagem>(new TicketMensagemMap().Configure);
            modelBuilder.Entity<PlanoComissao>(new PlanoComissaoMap().Configure);
            modelBuilder.Entity<ConectorOpenFinance>(new ConectorOpenFinanceMap().Configure);
            modelBuilder.Entity<Faturamento>(new FaturamentoMap().Configure);
            modelBuilder.Entity<ConfiguracaoConciliacao>(new ConfiguracaoConciliacaoMap().Configure);
            modelBuilder.Entity<ServicoNfse>(new ServicoNfseMap().Configure);
            modelBuilder.Entity<Nfse>(new NfseMap().Configure);
            modelBuilder.Entity<ConfiguracaoNfse>(new ConfiguracaoNfseMap().Configure);

        }
    }   
}
