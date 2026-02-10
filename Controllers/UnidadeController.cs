using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using ERP.Models;
using ERP.Domain.Entidades;
using System.Data.Entity;
using ERP_API.Models;
using System.Threading.Tasks;
using ERP_API.Service.Parceiros.Interface;
using ERP_API.Service.Parceiros;
using MySqlX.XDevAPI.Common;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class UnidadeController : ControllerBase
    {
        protected Context context;
        private readonly ITecnospeedService _tecnospeedService;
        public UnidadeController(Context context, ITecnospeedService tecnospeedSerivce)
        {
            this.context = context;
            _tecnospeedService = tecnospeedSerivce;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (usuarioLogado.TipoUsuario == "Administrador")
            {
                var result = context.Unidade
                    .Select(m => new
                    {
                        m.IdUnidade,
                        m.IdEmpresa,
                        m.Nome,
                        m.TipoPessoa,
                        m.RazaoSocial,
                        m.CpfCnpj,
                        m.Telefone1,
                        m.Telefone2,
                        m.Email,
                        m.Cep,
                        m.Logradouro,
                        m.Numero,
                        m.Complemento,
                        m.Bairro,
                        m.Cidade,
                        m.Estado,
                        m.Referencia,
                        m.InscricaoEstadual,
                        m.InscricaoMunicipal,
                        m.Situacao
                    }).ToList();
                return Ok(result);
            }
            else
            {

                var result = context.Unidade.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                      .Select(m => new
                      {
                          m.IdUnidade,
                          m.IdEmpresa,
                          m.Nome,
                          m.TipoPessoa,
                          m.RazaoSocial,
                          m.CpfCnpj,
                          m.Telefone1,
                          m.Telefone2,
                          m.Email,
                          m.Cep,
                          m.Logradouro,
                          m.Numero,
                          m.Complemento,
                          m.Bairro,
                          m.Cidade,
                          m.Estado,
                          m.Referencia,
                          m.InscricaoEstadual,
                          m.InscricaoMunicipal,
                          m.Situacao
                      }).ToList();
                return Ok(result);
            }

           
        }



        [HttpGet]
        [Route("listarAtivas")]
        public IActionResult ListarAtivas()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (usuarioLogado.TipoUsuario == "Administrador")
            {
                var result = context.Unidade
                 .Select(m => new
                 {
                     m.IdUnidade,
                     m.IdEmpresa,
                     m.Nome,
                     m.TipoPessoa,
                     m.RazaoSocial,
                     m.CpfCnpj,
                     m.Telefone1,
                     m.Telefone2,
                     m.Email,
                     m.Cep,
                     m.Logradouro,
                     m.Numero,
                     m.Complemento,
                     m.Bairro,
                     m.Cidade,
                     m.Estado,
                     m.Referencia,
                     m.InscricaoEstadual,
                     m.InscricaoMunicipal,
                     m.Situacao
                 }).ToList();
                return Ok(result);
            }
            else
            {
                var result = context.Unidade.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa && x.Situacao == "Ativo")
                      .Select(m => new
                      {
                          m.IdUnidade,
                          m.IdEmpresa,
                          m.Nome,
                          m.TipoPessoa,
                          m.RazaoSocial,
                          m.CpfCnpj,
                          m.Telefone1,
                          m.Telefone2,
                          m.Email,
                          m.Cep,
                          m.Logradouro,
                          m.Numero,
                          m.Complemento,
                          m.Bairro,
                          m.Cidade,
                          m.Estado,
                          m.Referencia,
                          m.InscricaoEstadual,
                          m.InscricaoMunicipal,
                          m.Situacao
                      }).ToList();
                return Ok(result);
            }
        }


        [HttpGet]
        [Route("listarPorEmpresa")]
        public IActionResult ListarPorEmpresa(int idEmpresa)
        {
            var result = context.Unidade.Where(x => x.IdEmpresa == idEmpresa && x.Situacao == "Ativo")
                  .Select(m => new
                  {
                      m.IdUnidade,
                      m.IdEmpresa,
                      m.Nome,
                      m.TipoPessoa,
                      m.RazaoSocial,
                      m.CpfCnpj,
                      m.Telefone1,
                      m.Telefone2,
                      m.Email,
                      m.Cep,
                      m.Logradouro,
                      m.Numero,
                      m.Complemento,
                      m.Bairro,
                      m.Cidade,
                      m.Estado,
                      m.Referencia,
                      m.InscricaoEstadual,
                      m.InscricaoMunicipal,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }


        [HttpGet]
        [Route("listarTodasUnidades")]
        public IActionResult ListarTodasUnidades()
        {

            var result = context.Unidade
                  .Select(m => new
                  {
                      m.IdUnidade,
                      m.IdEmpresa,
                      m.Nome,
                      m.TipoPessoa,
                      m.RazaoSocial,
                      m.CpfCnpj,
                      m.Telefone1,
                      m.Telefone2,
                      m.Email,
                      m.Cep,
                      m.Logradouro,
                      m.Numero,
                      m.Complemento,
                      m.Bairro,
                      m.Cidade,
                      m.Estado,
                      m.Referencia,
                      m.InscricaoEstadual,
                      m.InscricaoMunicipal,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarRegiao")]
        public IActionResult ListarRegiao()
        {
            var result = context.Regiao
                  .Select(m => new
                  {
                      m.IdRegiao,
                      m.NomeRegiao,

                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public async Task<IActionResult> Salvar([FromBody] UnidadeRequest model)
        {
            var parceiroParametro = context.ParceiroParametro.Include(x => x.ParceiroSistema).Where(x => x.ParceiroSistema.NomeParceiroSistema == "TecnoSpeed");

            var cnpjsh = parceiroParametro.FirstOrDefault(x => x.Chave == "cnpjsh").Valor ?? throw new Exception("Chave cnpjsh não encontrada");
            var tokensh = parceiroParametro.FirstOrDefault(x => x.Chave == "tokensh").Valor ?? throw new Exception("Chave tokensh não encontrada");
            var url = parceiroParametro.FirstOrDefault(x => x.Chave == "UrlBase").Valor ?? throw new Exception("Chave UrlBase não encontrada");

            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            Unidade unidade;

            if (model.IdUnidade > 0)
            {
                unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);
                unidade.Alterar(
                    empresa,
                    model.Nome,
                    model.TipoPessoa,
                    model.RazaoSocial,
                    model.CpfCnpj,
                    context.GrupoEmpresa.FirstOrDefault(x => x.IdGrupoEmpresa == model.IdGrupoEmpresa),
                    context.Regiao.FirstOrDefault(x => x.IdRegiao == model.IdRegiao),
                    context.RamoAtividade.FirstOrDefault(x => x.IdRamoAtividade == model.IdRamoAtividade),
                    model.Telefone1,
                    model.Telefone2,
                    model.Email,
                    model.Cep,
                    model.Logradouro,
                    model.Numero,
                    model.Complemento,
                    model.Bairro,
                    model.Cidade,
                    model.Estado,
                    model.Referencia,
                    model.InscricaoEstadual,
                    model.InscricaoMunicipal,
                    User.Identity.Name);

                AtualizarPagadorResponseModel retornoTecnoSpeed;
                try
                {
                     retornoTecnoSpeed = await AtualizarPagadorTecnoSpeed(unidade, cnpjsh, tokensh, url);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Erro ao atualizar pagador: {ex.Message}");
                }

                context.Update(unidade);
            }
            else
            {
                unidade = new Unidade(empresa, model.Nome, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, context.GrupoEmpresa.FirstOrDefault(x => x.IdGrupoEmpresa == model.IdGrupoEmpresa), context.Regiao.FirstOrDefault(x => x.IdRegiao == model.IdRegiao), context.RamoAtividade.FirstOrDefault(x => x.IdRamoAtividade == model.IdRamoAtividade), model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, User.Identity.Name);
                CadastroPagadorResponseModel retornoTecnoSpeed;
                CadastroPagadorResponseModel retornoPagadorExistente;
                try
                {
                    retornoTecnoSpeed = await CriarPagadorTecnoSpeed(unidade, cnpjsh, tokensh, url);
                }
                catch (Exception ex) 
                {
                    if (ex.Message != null && ex.Message.Contains("Pagador com CPF/CNPJ") && ex.Message.Contains("já cadastrado para esta SoftwareHouse"))
                    {
                        try
                        {
                            retornoPagadorExistente = await BuscarPagadorExistenteTecnoSpeed(model.CpfCnpj, cnpjsh, tokensh, url);

                            retornoTecnoSpeed = retornoPagadorExistente;
                        }
                        catch (Exception buscaError)
                        {

                            return BadRequest($"Erro ao recuperar dados do pagador: {buscaError.Message}");
                        }
                    }
                    else
                    {
                        return BadRequest($"Erro ao cadastrar pagador: {ex.Message}");
                    }
                }
                unidade.SetDadosTecnoSpeed(retornoTecnoSpeed.Token);

                context.Unidade.Add(unidade);
            }
            context.SaveChanges();
            return Ok();
        }

        private async Task<AtualizarPagadorResponseModel> AtualizarPagadorTecnoSpeed(Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var retorno = new AtualizarPagadorResponseModel();
            try
            {
                var request = new AtualizarPagadorRequestModel()
                {
                    Name = unidade.Nome,
                    Email = unidade.Email,
                    Street = unidade.Logradouro,
                    Neighborhood = unidade.Bairro,
                    AddressNumber = unidade.Numero,
                    AddressComplement = unidade.Complemento,
                    City = unidade.Cidade,
                    State = unidade.Estado,
                    Zipcode = unidade.Cep
                };

                retorno = await _tecnospeedService.AtualizarPagadorTecnospeed(request, unidade, cnpjsh, tokensh, url);
                return retorno;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<CadastroPagadorResponseModel> CriarPagadorTecnoSpeed(Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var retorno = new CadastroPagadorResponseModel();
            
            

                var request = new CadastroPagadorRequestModel()
                {
                    name = unidade.Nome,
                    email = unidade.Email,
                    cpfCnpj = unidade.CpfCnpj,
                    neighborhood = unidade.Bairro,
                    street = unidade.Logradouro,
                    addressNumber = unidade.Numero,
                    addressComplement = unidade.Complemento,
                    ddaActivated = true,
                    city = unidade.Cidade,
                    state = unidade.Estado,
                    zipcode = unidade.Cep
                };
                retorno = await _tecnospeedService.CadastroPagadorTecnospeed(request, cnpjsh, tokensh, url);
                return retorno;
        }

        private async Task<CadastroPagadorResponseModel> BuscarPagadorExistenteTecnoSpeed(string cnpjPagador, string cnpjsh, string tokensh, string url)
        {
            var retorno = new CadastroPagadorResponseModel();

            retorno = await _tecnospeedService.BuscarPagadorTecnospeed(cnpjPagador, cnpjsh, tokensh, url);
            return retorno;
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.Unidade.AsQueryable();
            switch (model.Chave)
            {
                case "Nome":
                    result = result.Where(x => x.Nome.Contains(model.Valor.ToUpper()));
                    break;
                case "RazaoSocial":
                    result = result.Where(x => x.RazaoSocial.Contains(model.Valor.ToUpper()));
                    break;
                case "CpfCnpj":
                    result = result.Where(x => x.CpfCnpj == model.Valor);
                    break;
                default:
                    // code block
                    break;
            }

            return Ok(result.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa).Select(m => new
            {
                m.IdUnidade,
                m.IdEmpresa,
                m.Nome,
                m.RazaoSocial,
                m.CpfCnpj,
                m.Situacao
            }).Take(500).ToList());
        }



        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == id);
            unidade.Excluir(User.Identity.Name);

            context.Update(unidade);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var unidade = context.Unidade.Include(x => x.Empresa).FirstOrDefault(x => x.IdUnidade == id);
            if (unidade == null)
                return BadRequest("Unidade não encontrada ");

            return Ok(new UnidadeResponse()
            {
                IdUnidade = unidade.IdUnidade,
                IdEmpresa = unidade.IdEmpresa,
                Nome = unidade.Nome,
                TipoPessoa = unidade.TipoPessoa,
                RazaoSocial = unidade.RazaoSocial,
                CpfCnpj = unidade.CpfCnpj,
                IdGrupoEmpresa = unidade.IdGrupoEmpresa,
                IdRegiao = unidade.IdRegiao,
                //NomeRegiao = empresa.Regiao.NomeRegiao,
                IdRamoAtividade = unidade.IdRamoAtividade,
                Telefone1 = unidade.Telefone1,
                Telefone2 = unidade.Telefone2,
                Email = unidade.Email,
                Cep = unidade.Cep,
                Logradouro = unidade.Logradouro,
                Numero = unidade.Numero,
                Complemento = unidade.Complemento,
                Bairro = unidade.Bairro,
                Cidade = unidade.Cidade,
                Estado = unidade.Estado,
                Referencia = unidade.Referencia,
                InscricaoEstadual = unidade.InscricaoEstadual,
                InscricaoMunicipal = unidade.InscricaoMunicipal,
                Situacao = unidade.Situacao
            });
        }


    }
}
