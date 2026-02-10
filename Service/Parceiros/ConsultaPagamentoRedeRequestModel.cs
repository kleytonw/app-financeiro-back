namespace ERP_API.Service.Parceiros
{
    public class ConsultaPagamentoRedeRequestModel
    {
        /// <summary>
        /// Token de acesso ao serviço (Authorization Header)
        /// </summary>
        public string Authorization { get; set; }

        /// <summary>
        /// Número do Ponto de Venda (PV). Tamanho Máximo: 9
        /// </summary>
        public string ParentCompanyNumber { get; set; }
        /// <summary>
        /// Url de chamada da requisição 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Lista de Pontos de Venda Filiais separados por vírgula (,).
        /// </summary>
        public string Subsidiaries { get; set; }

        /// <summary>
        /// Data de início da consulta. O período entre startDate e endDate não deve ser superior a 30 dias.
        /// Formato esperado: yyyy-MM-dd (exemplo: 2018-09-02)
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Data de fim da consulta. O período entre startDate e endDate não deve ser superior a 30 dias.
        /// Formato esperado: yyyy-MM-dd (exemplo: 2018-09-15)
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Quantidade de ocorrências no retorno. Limite: 100.
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// Identificador da próxima página, para paginação. Tamanho máximo: 750 caracteres.
        /// </summary>
        public string PageKey { get; set; }

        /// <summary>
        /// Código da bandeira de gestão de vendas.
        /// </summary>
        public int? Brands { get; set; }

        /// <summary>
        /// Status do pagamento. Tamanho máximo: 15 caracteres.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Descrição do tipo de pagamento. Tamanho máximo: 15 caracteres.
        /// </summary>
        public string Types { get; set; }
    }

}
