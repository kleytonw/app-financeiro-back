using System.ComponentModel.DataAnnotations;
using System;

namespace ERP_API.Service.Parceiros
{
    public class ConsultarExtratoPorPeriodoTecnospeedRequestModel
    {
        [Required]
        public DateTime DateStart { get; set; } 

        [Required]
        public DateTime DateEnd { get; set; } 

        [StringLength(3, ErrorMessage = "O código do banco deve ter no máximo 3 caracteres.")]
        public string BankCode { get; set; } 

        [RegularExpression("ofx|ret", ErrorMessage = "O tipo deve ser 'ofx' ou 'ret'.")]
        public string Type { get; set; } 

        public string AccountHash { get; set; } 

        [Range(1, int.MaxValue, ErrorMessage = "O número da página deve ser maior que zero.")]
        public int? Page { get; set; } 

        [Range(1, int.MaxValue, ErrorMessage = "O limite deve ser maior que zero.")]
        public int? Limit { get; set; } 
    }

}
