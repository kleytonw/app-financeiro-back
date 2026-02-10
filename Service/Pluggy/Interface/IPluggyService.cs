using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Models.Pluggy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP_API.Service.Pluggy.Interface
{
    public interface IPluggyService
    {
        /// <summary>
        /// Autentica na API Pluggy e retorna o accessToken.
        /// </summary>
        Task<string> AuthenticateAsync();

        /// <summary>
        /// Cria um connectToken usado pelo front-end (PluggyConnect).
        /// </summary>
        Task<string> CreateConnectTokenAsync(); // string clienteUserId, string itemId

        /// <summary>
        /// Obtém detalhes de um item (conta conectada) pelo ID.
        /// </summary>
        Task<GetItemResponseModel> GetItemAsync(string itemId);

        Task<PluggyConnectResponse> CreateItemPessoalAsync(CreateItemPessoalPluggyRequestModel request);
        Task<PluggyConnectResponse> CreateItemEmpresarialAsync(CreateItemEmpresarialPluggyRequestModel request);


        /// <summary>
        /// Lista todas as contas associadas a um item Pluggy.
        /// </summary>
        Task<ListaContasResponse> GetAccountsAsync(string itemId);

        /// <summary>
        /// Lista transações de uma conta Pluggy por período.
        /// </summary>
        Task<PluggyAccountResponse?> GetAccountByIdAsync(string accountId);

        Task<List<TransactionDto>> GetAllTransactionsAsync(
        Guid? accountId = null,
        DateTime? from = null,
        DateTime? to = null,
        string? ids = null,
        int pageSize = 500);


        // Atualiza dados conta 
        Task<string> UpdateItemAsync(string itemid);

        Task<List<ConectorOpenFinanceResponseModel>> GetAllConnectors();

    }
}
