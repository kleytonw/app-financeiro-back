using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ERP_API.Models;

namespace ERP_API.Service.Parceiros.Interface
{
    public interface IConciliadoraService
    {
        // Métodos principais de envio
        Task<(bool Success, string ErrorMessage, ErroXmlResponse Data)> EnviarVendaSistemaAsync(string senha, string idEmpresa, IFormFile arquivoXml);
        Task<(bool Success, string ErrorMessage, AdquirentesConciliadoraResponse Data)> ListaAdquirenteConciliadoraResponse1(string apiKey, int? top, int? skip);
        Task<(bool Success, string ErrorMessage, VendasCanceladasConciladoraResponse Data)> VendasCanceladas(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu);
        Task<(bool Success, string ErrorMessage, ConciliacaoSistemaConciliadoraResponse Data)> ConciliacaoSistema(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu);
        Task<(bool Success, string ErrorMessage, PagamentosConciliadoraResponse Data)> PagamentosRecebidos(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu);
        Task<(bool Success, string ErrorMessage, PrevisaoPagamentoConciliadoraResponse Data)> PrevisaoPagamento(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu);
        Task<(bool Success, string ErrorMessage, List<StatusProcessamentoResponse> Data)> StatusProcessamento(StatusProcessamentoConciladoraRequest model);
        Task<(bool Success, string ErrorMessage, VendasConciliadoraResponse Data)> Vendas(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu, int? modalidade);
        Task<(bool Success, string ErrorMessage, VendasConciliadasConciliadoraResponse Data)> VendasConciliadas(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu, int? modalidade);
        Task<(bool Success, string ErrorMessage, VendasNaoEnviadasConciliadoraResponse Data)> VendasNaoEnviadas(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu, int? modalidade);
        Task<(bool Success, string ErrorMessage, MeioCapturaConciliadoraResponse Data)> MeioCaptura(string apiKey, int? top, int? skip);
        Task<(bool Success, string ErrorMessage, ModalidadeConciliadoraResponse Data)> Modalidade(string apiKey, int? top, int? skip);
        Task<(bool Success, string ErrorMessage, ProdutoConciliadoraResponse Data)> Produto(string apiKey, int? top, int? skip);
    }
}
