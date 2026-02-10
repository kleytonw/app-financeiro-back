using System.ComponentModel.DataAnnotations;
using System;

namespace ERP_API.Service.Parceiros
{
    public class BilhetagemTecnospeedRequestModel
    {
        public DateTime DataInicial { get; set; }

        [Required]
        public DateTime DataFinal { get; set; }

        [Range(1, 1000, ErrorMessage = "O limite deve estar entre 1 e 1000.")]
        public int? Limit { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A página deve ser um valor positivo.")]
        public int? Page { get; set; }
    }
}
