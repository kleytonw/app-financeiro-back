using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ERP_API.Service.Parceiros
{
    public class EnviarExtratoTecnospeedRequestModel
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
