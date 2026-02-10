using ERP.Infra;
using ERP_API.Models;
using ERP_API.Models.NotaFiscal;
using ERP_Application.Models.Parceiros.IntegraNotas;
using ERP_Application.Services.Parceiros.IntegraNotas.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sdk.CloudDfe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Application.Services.Parceiros
{
    public class IntegraNotasService : IIntegraNotasService
    {
        //private readonly IRepositorioNotaFiscal _repositorioNotaFiscal;
        //private readonly IRepositorioLogIntegracao _repositorioLogIntegracao;
        private readonly IConfiguration _configuration;
        private readonly Context _context;

        //public string _token { get; set; }
        public string _ambiente { get; set; }


        public IntegraNotasService(IConfiguration configuration, Context context)
            //IUsuarioAplicacao pUsuarioAplicacao,
            //IRepositorioNotaFiscal pRepositorioNotaFiscal, IRepositorioLogIntegracao repositorioLogIntegracao) : base(pUsuarioAplicacao)
        {
            _configuration = configuration;
            _context = context;

            //_repositorioNotaFiscal = pRepositorioNotaFiscal;
            //_token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbXAiOjEyODQ2LCJ1c3IiOjQ4NCwidHAiOjIsImlhdCI6MTc2MTY3MjgzM30._zUyeyWSONHAy6Y6WMEz1ebt7akX7AUsUCpviBX60Aw";
            _ambiente = "Prod"; // homolog
            //_repositorioLogIntegracao = repositorioLogIntegracao;
        }

        public async Task<Response<CancelarNFseResponseModel>> CancelarNFseAsync(CancelarNFseRequestModel pCancelarRequestModel, string _token)
        {
            var lRetorno = new Response<CancelarNFseResponseModel>()
            {
                Data = new CancelarNFseResponseModel()
            };

            var lConfig = new Dictionary<string, object>
            {
                { "token", _token},
                { "ambiente", this._ambiente == "Prod" ?  Consts.AMBIENTE_PRODUCAO : Consts.AMBIENTE_HOMOLOGACAO },
                { "timeout", 60 },
                { "debug", true }
            };

            var lNfse = new Nfse(lConfig);

            var lPayload = new Dictionary<string, object>
            {
                {"chave", pCancelarRequestModel.Chave },
                {"codigo_cancelamento", pCancelarRequestModel.CodigoCancelamento},
            };

            try
            {
                //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse/cancela", JsonConvert.SerializeObject(lPayload)!, true, TipoIntegracao.Request));
                //await _repositorioLogIntegracao.SaveChangesAsync();

                var lResp = Task.Run(async () => await lNfse.Cancela(lPayload)).GetAwaiter().GetResult();
                
                if (lResp.ContainsKey("erros"))
                {
                    lRetorno.Success = false;
                    lRetorno.Message = "Cancelamento não realizado";
                } else
                {
                    lRetorno.Success = true;
                    lRetorno.Message = "Cancelamento realizado com sucesso";
                }

                string lJsonOutput = JsonConvert.SerializeObject(lResp, Formatting.Indented);
                lRetorno.Data = JsonConvert.DeserializeObject<CancelarNFseResponseModel>(lJsonOutput)!;

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro ao obter o status: {ex.Message}");
            }

            return lRetorno;
        }

        public async Task<Response<PdfNFseResponseModel>> GerarPdfNFseAsync(string chave, string _token)
        {
            var lRetorno = new Response<PdfNFseResponseModel>()
            {
                Data = new PdfNFseResponseModel()
            };

            var lConfig = new Dictionary<string, object>
            {
                { "token", _token },
                { "ambiente", this._ambiente == "Prod" ?  Consts.AMBIENTE_PRODUCAO : Consts.AMBIENTE_HOMOLOGACAO },
                { "timeout", 60 },
                { "debug", true }
            };

            var nfse = new Nfse(lConfig);

            var lPayload = new Dictionary<string, object>
            {
                {"chave", chave}
            };

            var resp = Task.Run(async () => await nfse.Pdf(lPayload)).GetAwaiter().GetResult();

            string lJsonOutput = JsonConvert.SerializeObject(resp, Formatting.Indented);
            lRetorno.Data = JsonConvert.DeserializeObject<PdfNFseResponseModel>(lJsonOutput)!;

            lRetorno.Success = true;
            lRetorno.Message = "Pdf gerado com sucesso!";
            return lRetorno;
        }

        //public async Task<Response<ConsultarNFseResponseModel>> ConsultarNFseAsync(ConsultarNFseRequestModel pConsultarNFseRequestModel)
        public async Task<Response<ConsultarNFseResponseModel>> ConsultarNFseAsync(string chave, string _token)
        {
            var lRetorno = new Response<ConsultarNFseResponseModel>()
            {
                Data = new ConsultarNFseResponseModel()
            };

            var lConfig = new Dictionary<string, object>
            {
                { "token", _token },
                { "ambiente", this._ambiente == "Prod" ?  Consts.AMBIENTE_PRODUCAO : Consts.AMBIENTE_HOMOLOGACAO },
                { "timeout", 60 },
                { "debug", true }
            };

            var nfse = new Nfse(lConfig);

            var lPayload = new Dictionary<string, object>
            {
                {"chave", chave}
            };

            try
            {
                //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse/{chave}", JsonConvert.SerializeObject(lPayload)!, true, TipoIntegracao.Request));
                //await _repositorioLogIntegracao.SaveChangesAsync();

                var resp = Task.Run(async () => await nfse.Consulta(lPayload)).GetAwaiter().GetResult();

                string lJsonOutput = JsonConvert.SerializeObject(resp, Formatting.Indented);
                lRetorno.Data = JsonConvert.DeserializeObject<ConsultarNFseResponseModel>(lJsonOutput)!;
            }
            catch (ArgumentException ex)
            {
                throw new Exception($"{ex.Message}");
            }

            lRetorno.Success = true;
            lRetorno.Message = "Consulta NFSe" + chave;
            return lRetorno;
        }

        public async Task<Response<CriarEmitenteIntegraNotasResponseModel>> CriarEmitenteAsync(CriarEmitenteIntegraNotasRequestModel pEmitenteRequestModel, string _token)
        {
            var lRetorno = new Response<CriarEmitenteIntegraNotasResponseModel>()
            {
                Data = new CriarEmitenteIntegraNotasResponseModel()
            };
            var lConfig = new Dictionary<string, object>
            {
                // token abaixo é da integranotas, e é usado apenas nesse momento. Demais situações, deve ser usado o token do EMITENTE
                { "token", _token },
                { "ambiente", Consts.AMBIENTE_PRODUCAO },
                { "timeout", 60 },
                { "debug", true }
            };

            var lSoftPay = new Softhouse(lConfig);

            var lPayload = new Dictionary<string, object>
            {
                { "nome", pEmitenteRequestModel.Nome },
                { "razao", pEmitenteRequestModel.RazaoSocial },
                { "cnpj", pEmitenteRequestModel.Cnpj },
                //{ "cnae", pClienteRequestModel.Cnae },
                { "crt", pEmitenteRequestModel.Crt }, // Lucro presumido, simples etc.
                { "im", pEmitenteRequestModel.InscricaoMunicipal },

                { "login_prefeitura", pEmitenteRequestModel.LoginPrefeitura },
                { "senha_prefeitura", pEmitenteRequestModel.SenhaPrefeitura },
                { "client_id_prefeitura", pEmitenteRequestModel.ClientIdPrefeitura },
                { "client_secret_prefeitura",  pEmitenteRequestModel.ClientSecretPrefeitura },

                { "telefone", pEmitenteRequestModel.Telefone },
                { "email", pEmitenteRequestModel.Email },

                { "rua", pEmitenteRequestModel.Endereco.Rua },
                { "numero", pEmitenteRequestModel.Endereco.Numero },
                { "complemento", pEmitenteRequestModel.Endereco.Complemento },
                { "bairro", pEmitenteRequestModel.Endereco.Bairro },
                { "municipio", pEmitenteRequestModel.Endereco.Municipio },
                { "cmun", pEmitenteRequestModel.Endereco.CodigoIbge }, // Código IBGE
                { "uf", pEmitenteRequestModel.Endereco.Uf },
                { "cep", pEmitenteRequestModel.Endereco.Cep },

                { "logo", pEmitenteRequestModel.LogoBase64 },

                { "documentos", new Dictionary<string, object>
                    {
                        { "nfse", true }
                    }
                }
            };

            //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/soft/emitente", JsonConvert.SerializeObject(lPayload)!, true, TipoIntegracao.Request));
            //await _repositorioLogIntegracao.SaveChangesAsync();

            var llResp = Task.Run(async () => await lSoftPay.CriaEmitente(lPayload)).GetAwaiter().GetResult();
            string llJsonOutput = JsonConvert.SerializeObject(llResp, Formatting.Indented);

            lRetorno.Success = true;
            lRetorno.Message = "Criação de emitente realizada comm sucesso." + llJsonOutput;
            return lRetorno;
        }

        public async Task<Response<NfseResponseModel>> CriarNFseAsync(NfseRequestRequestModel pCriarNfse, string _token)
        {
            var lRetorno = new Response<NfseResponseModel>()
            {
                Data = new NfseResponseModel()
            };

            var config = new Dictionary<string, object>
            {
                { "token", _token },
                { "ambiente", this._ambiente == "Prod" ?  Consts.AMBIENTE_PRODUCAO : Consts.AMBIENTE_HOMOLOGACAO },
                { "timeout", 60 },
                { "debug", true }
            };

            var nfse = new Nfse(config);

            var lPayload = new Dictionary<string, object>
            {
                { "numero", pCriarNfse.Numero },
                { "serie", pCriarNfse.Serie },
                { "status", pCriarNfse.Status },
                { "data_emissao", pCriarNfse.DataEmissao },
                { "tomador", new Dictionary<string, object>
                    {
                        { "cpf", pCriarNfse.Tomador?.Cpf?.Replace(".", "").Replace("/", "").Replace("-", "")! },
                        { "cnpj", pCriarNfse.Tomador?.Cnpj?.Replace(".", "").Replace("/", "").Replace("-", "")! },
                        { "razao_social", pCriarNfse.Tomador?.RazaoSocial! },
                        { "email", pCriarNfse.Tomador?.Email! },
                        { "telefone", pCriarNfse.Tomador?.Telefone!},
                        { "endereco", new Dictionary<string, object>
                            {
                                { "logradouro", pCriarNfse.Tomador?.Endereco?.Logradouro! },
                                { "numero", pCriarNfse.Tomador?.Endereco?.Numero! },
                                { "complemento", pCriarNfse.Tomador?.Endereco?.Complemento! },
                                { "bairro", pCriarNfse.Tomador?.Endereco?.Bairro! },
                                { "codigo_municipio", pCriarNfse.Tomador?.Endereco?.CodigoMunicipio! },
                                { "nome_municipio", pCriarNfse.Tomador?.Endereco?.NomeMunicipio! },
                                { "uf", pCriarNfse.Tomador?.Endereco?.Uf! },
                                { "cep", pCriarNfse.Tomador?.Endereco?.Cep?.Replace("-", "")! }
                            }
                        }
                    }
                },
                { "servico", new Dictionary<string, object>
                    {
                        { "codigo_municipio", pCriarNfse.Servico?.CodigoMunicipio!  },
                        { "codigo_municipio_prestacao", pCriarNfse.Servico?.CodigoMunicipioPrestacao! },
                        { "itens", new List<Dictionary<string, object>>
                            {
                                new Dictionary<string, object>
                                {
                                    { "codigo", pCriarNfse.Servico?.Codigo! },
                                    { "codigo_nbs", "115022000" },
                                    { "codigo_tributacao_municipio", pCriarNfse.Servico?.CodigoTributacaoMunicipio! },
                                    { "discriminacao", pCriarNfse.Servico?.Discriminacao! },
                                    { "unidade_nome", pCriarNfse.Servico?.UnidadeNome! },
                                    { "unidade_quantidade", pCriarNfse.Servico?.UnidadeQuantidade! },
                                    { "unidade_valor", pCriarNfse.Servico?.ValorServicos! },
                                    { "valor_bruto", pCriarNfse.Servico?.ValorBruto! },
                                    { "valor_liquido", pCriarNfse.Servico?.ValorLiquido! },
                                    { "valor_servicos", pCriarNfse.Servico?.ValorServicos! },
                                    { "valor_base_calculo_iss", pCriarNfse.Servico?.ValorBaseCalculoIss! },
                                    { "aliquota_iss", pCriarNfse.Servico?.AliquotaIss! },
                                    { "valor_aliquota", pCriarNfse.Servico?.AliquotaIss! },
                                    { "valor_iss", pCriarNfse.Servico?.ValorIss! }
                                }
                            }
                        }
                    }
                },
                { "informacoes_complementares", pCriarNfse.InformacoesComplementares! }
            };


            //var lPayload = new Dictionary<string, object>
            //{
            //    { "numero", pCriarNfse.Numero },
            //    { "serie", pCriarNfse.Serie },
            //    { "status", pCriarNfse.Status },
            //    { "data_emissao", pCriarNfse.DataEmissao },
            //    { "tomador", new Dictionary<string, object>
            //        {
            //            { "cpf", pCriarNfse.Tomador?.Cpf?.Replace(".", "").Replace("/", "").Replace("-", "")! },
            //            { "cnpj", pCriarNfse.Tomador?.Cnpj?.Replace(".", "").Replace("/", "").Replace("-", "")! },
            //            { "razao_social", pCriarNfse.Tomador?.RazaoSocial! },
            //            { "email", pCriarNfse.Tomador?.Email! },
            //            { "endereco", new Dictionary<string, object>
            //                {
            //                    { "logradouro", pCriarNfse.Tomador?.Endereco?.Logradouro! },
            //                    { "numero", pCriarNfse.Tomador?.Endereco?.Numero! },
            //                    { "complemento", pCriarNfse.Tomador?.Endereco?.Complemento! },
            //                    { "bairro", pCriarNfse.Tomador?.Endereco?.Bairro! },
            //                    { "codigo_municipio", pCriarNfse.Tomador?.Endereco?.CodigoMunicipio! },
            //                    { "nome_municipio", pCriarNfse.Tomador?.Endereco?.NomeMunicipio! },
            //                    { "uf", pCriarNfse.Tomador?.Endereco?.Uf! },
            //                    { "cep", pCriarNfse.Tomador?.Endereco?.Cep?.Replace("-", "")! }
            //                }
            //            }
            //        }
            //    },
            //    { "servico", new Dictionary<string, object>
            //        {
            //            { "codigo_municipio", pCriarNfse.Servico?.CodigoMunicipio!  },
            //            { "codigo_municipio_prestacao", pCriarNfse.Servico?.CodigoMunicipioPrestacao! },
            //            { "codigo", pCriarNfse.Servico?.Codigo! },
            //            { "valor_servicos", pCriarNfse.Servico?.ValorServicos! },
            //            { "discriminacao", pCriarNfse.Servico?.Discriminacao! },
            //            { "codigo_tributacao_municipio", pCriarNfse.Servico?.CodigoTributacaoMunicipio! },
            //            { "unidade_nome", pCriarNfse.Servico?.UnidadeNome! },
            //            { "unidade_quantidade", pCriarNfse.Servico?.UnidadeQuantidade! },
            //            { "unidade_valor", pCriarNfse.Servico?.ValorServicos! },
            //            { "valor_bruto", pCriarNfse.Servico?.ValorBruto! },
            //            { "valor_liquido", pCriarNfse.Servico?.ValorLiquido! },
            //            { "valor_base_calculo_iss", pCriarNfse.Servico?.ValorBaseCalculoIss! },
            //            { "aliquota_iss", pCriarNfse.Servico?.AliquotaIss! },
            //            { "valor_iss", pCriarNfse.Servico?.ValorIss! }
            //        }
            //    },
            //    { "informacoes_complementares", pCriarNfse.InformacoesComplementares! }
            //};


            //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse", JsonConvert.SerializeObject(lPayload)!, true, TipoIntegracao.Request));
            //await _repositorioLogIntegracao.SaveChangesAsync();

            string json = JsonConvert.SerializeObject(lPayload, Formatting.Indented);

            var lResp = Task.Run(async () => await nfse.Cria(lPayload)).GetAwaiter().GetResult();
            string lJsonOutput = String.Empty;

            if (lResp.ContainsKey("sucesso") && (bool)lResp["sucesso"])
            {
                var chave = lResp["chave"].ToString();
                var payloadConsulta = new Dictionary<string, object> { { "chave", chave } };

                // Thread.Sleep(15000);

                var lRespC = Task.Run(async () => await nfse.Consulta(payloadConsulta)).GetAwaiter().GetResult();

                if (!lRespC.ContainsKey("codigo") || Convert.ToInt32(lRespC["codigo"]) != 5023)
                {
                    if (lRespC.ContainsKey("sucesso") && (bool)lRespC["sucesso"])
                    {
                        // autorizado
                        lJsonOutput = JsonConvert.SerializeObject(lRespC, Formatting.Indented);
                        //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse", lJsonOutput!, true, TipoIntegracao.Response));
                        //await _repositorioLogIntegracao.SaveChangesAsync();

                        var lAutorizado = JsonConvert.DeserializeObject<NfseResponseAutorizadoModel>(lJsonOutput)!;

                        lRetorno.Data.Chave = chave;
                        lRetorno.Data.Sucesso = true;
                        lRetorno.Data.StatusNotaFiscal = ERP_API.Models.StatusNotaFiscal.Autorizado;

                    }
                    else
                    {
                        // rejeitação
                        lJsonOutput = JsonConvert.SerializeObject(lRespC, Formatting.Indented);
                        //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse", lJsonOutput!, true, TipoIntegracao.Response));
                        //await _repositorioLogIntegracao.SaveChangesAsync();

                        lRetorno.Data.Chave = chave;
                        lRetorno.Data.Sucesso = true;
                        lRetorno.Data.StatusNotaFiscal = ERP_API.Models.StatusNotaFiscal.Rejeitado;
                    }
                }
                else
                {
                    // nota em processamento
                    // recomendamos que seja utilizado  o metodo de consulta manual ou o webhook
                    lJsonOutput = JsonConvert.SerializeObject(lRespC, Formatting.Indented);
                    //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse", lJsonOutput!, true, TipoIntegracao.Response));
                    //await _repositorioLogIntegracao.SaveChangesAsync();

                    lRetorno.Data.Chave = chave;
                    lRetorno.Data.StatusNotaFiscal = ERP_API.Models.StatusNotaFiscal.EmProcessamento;
                    lRetorno.Data.Sucesso = true;
                }
            }
            else if (lResp.ContainsKey("codigo") && (Convert.ToInt32(lResp["codigo"]) == 5001 || Convert.ToInt32(lResp["codigo"]) == 5002))
            {
                if (lResp.ContainsKey("erros"))
                {
                    lRetorno.Data.Erros = lResp["erros"].ToString();
                }
                lRetorno.Data.StatusNotaFiscal = ERP_API.Models.StatusNotaFiscal.Erro;
                lRetorno.Data.Sucesso = false;

            }
            else if (lResp.ContainsKey("codigo") && (Convert.ToInt32(lResp["codigo"]) == 5008))
            {
                var chave = lResp["chave"].ToString();
                string jsonOutput = JsonConvert.SerializeObject(lResp, Formatting.Indented);
                //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse", jsonOutput!, true, TipoIntegracao.Response));
                //await _repositorioLogIntegracao.SaveChangesAsync();

                var payloadConsulta = new Dictionary<string, object> { { "chave", chave! } };
                var lRespC = Task.Run(async () => await nfse.Consulta(payloadConsulta)).GetAwaiter().GetResult();

                if (!lRespC.ContainsKey("codigo") || Convert.ToInt32(lRespC["codigo"]) != 5023)
                {
                    if (lRespC.ContainsKey("sucesso") && (bool)lRespC["sucesso"])
                    {
                        lJsonOutput = JsonConvert.SerializeObject(lRespC, Formatting.Indented);
                        //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse", lJsonOutput!, true, TipoIntegracao.Response));
                        //await _repositorioLogIntegracao.SaveChangesAsync();

                        lRetorno.Data.StatusNotaFiscal = ERP_API.Models.StatusNotaFiscal.Autorizado;
                        lRetorno.Data.Chave = chave;
                        lRetorno.Data.Sucesso = true;
                    }
                    else
                    {
                        lJsonOutput = JsonConvert.SerializeObject(lRespC, Formatting.Indented);
                        //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse", lJsonOutput!, true, TipoIntegracao.Response));
                        //await _repositorioLogIntegracao.SaveChangesAsync();

                        lRetorno.Data.StatusNotaFiscal = ERP_API.Models.StatusNotaFiscal.Rejeitado;
                        lRetorno.Data.Chave = chave;
                        lRetorno.Data.Sucesso = true;
                    }
                }
                else
                {
                    lJsonOutput = JsonConvert.SerializeObject(lRespC, Formatting.Indented);
                    //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse", lJsonOutput!, true, TipoIntegracao.Response));
                    //await _repositorioLogIntegracao.SaveChangesAsync();

                    lRetorno.Data.StatusNotaFiscal = ERP_API.Models.StatusNotaFiscal.EmProcessamento;
                    lRetorno.Data.Sucesso = true;
                }
            }
            else
            {
                if (lResp.ContainsKey("chave"))
                {
                    lRetorno.Data.Chave = lResp["chave"].ToString();
                }

                string jsonOutput = JsonConvert.SerializeObject(lResp, Formatting.Indented);
                //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse", lJsonOutput!, true, TipoIntegracao.Response));
                //await _repositorioLogIntegracao.SaveChangesAsync();

                lRetorno.Data.StatusNotaFiscal = ERP_API.Models.StatusNotaFiscal.EmProcessamento;
                lRetorno.Data.Sucesso = true;
            }

            lRetorno.Success = true;
            lRetorno.Message = "Criação nota fiscal de serviço realizada com sucesso.";
            return lRetorno;
        }

        public async Task<Response<BuscarNFseResponseModel>> BuscarNFseAsync(BuscarNfseRequestModel pBuscaNfseRequestModel, string _token)
        {
            var lRetorno = new Response<BuscarNFseResponseModel>()
            {
                Data = new BuscarNFseResponseModel()
            };

            var config = new Dictionary<string, object>
            {
                { "token", _token },
                { "ambiente", this._ambiente == "Prod" ?  Consts.AMBIENTE_PRODUCAO : Consts.AMBIENTE_HOMOLOGACAO },
                { "timeout", 60 },
                { "debug", true }
            };

            var nfse = new Nfse(config);
            var lPayload = new Dictionary<string, object>
            {
                {"numero_inicial", "1"},
                {"numero_final", "5000000"},
                {"serie", "1"},
                // {"data_inicial", "2019-12-01"},
                // {"data_final", "2019-12-31"},
                // {"cancel_inicial", "2019-12-01"} // - Cancelamento
                // {"cancel_final", "2019-12-31"}
            };

            try
            {

                //await _repositorioLogIntegracao.AdicionarLogAsync(new LogIntegracao(null!, "https://api.integranotas.com.br/v1/nfse/busca", JsonConvert.SerializeObject(lPayload)!, true, TipoIntegracao.Request));
                //await _repositorioLogIntegracao.SaveChangesAsync();


                var lResp = Task.Run(async () => await nfse.Busca(lPayload)).GetAwaiter().GetResult();


                string lJsonOutput = JsonConvert.SerializeObject(lResp, Formatting.Indented);
                lRetorno.Data = JsonConvert.DeserializeObject<BuscarNFseResponseModel>(lJsonOutput)!;
            }
            catch (ArgumentException ex)
            {
                throw new Exception($"Erro ao obter o status: {ex.Message}");
            }

            lRetorno.Success = true;
            lRetorno.Message = "Consulta..";

            return lRetorno;
        }

    }

}

