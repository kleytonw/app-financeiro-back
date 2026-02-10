using System.ComponentModel.DataAnnotations;

namespace ERP_API.Service.Parceiros
{
    public class AtualizarPagadorRequestModel
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string Street { get; set; }

        [Required]
        [StringLength(250)]
        public string Neighborhood { get; set; }

        [Required]
        [StringLength(10)]
        public string AddressNumber { get; set; }

        [StringLength(250)]
        public string AddressComplement { get; set; }

        [Required]
        [StringLength(250)]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [StringLength(10)]
        public string Zipcode { get; set; }
    }
}
