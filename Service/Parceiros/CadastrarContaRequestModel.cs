using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP_API.Service.Parceiros
{
    public class CadastrarContaRequestListModel
    {
        public List<CadastrarContaRequestModel> Accounts { get; set; }
    }
    public class CadastrarContaRequestModel
    {
        [Required]
        [StringLength(4)]
        public string bankCode { get; set; }

        [Required]
        [StringLength(10)]
        public string agency { get; set; }

        [StringLength(1)]
        public string agencyDigit { get; set; }

        [Required]
        [StringLength(12)]
        public string accountNumber { get; set; }

        [StringLength(1)]
        public string accountNumberDigit { get; set; }

        [StringLength(2)]
        public string accountDac { get; set; }

        public int accountType { get; set; }

        public bool accountsPayment { get; set; }

        public decimal? remessaSequential { get; set; }

        [StringLength(20)]
        public string conventionNumber { get; set; }

        public bool? virtualAccount { get; set; }

        [StringLength(100)]
        public string bankAccountContract { get; set; }

        public bool? ddaActivated { get; set; }

        [StringLength(100)]
        public string clientKey { get; set; }

        [StringLength(100)]
        public string clientId { get; set; }

        [StringLength(100)]
        public string clientName { get; set; }

        public bool? recipientIdentification { get; set; }
    }
}
