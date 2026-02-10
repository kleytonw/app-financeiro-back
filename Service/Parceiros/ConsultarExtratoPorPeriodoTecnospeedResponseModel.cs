using System.Collections.Generic;
using System;

namespace ERP_API.Service.Parceiros
{
    public class ConsultarExtratoPorPeriodoTecnospeedResponseModel
    {
        public List<ExtratoData> Data { get; set; } 
        public Meta Meta { get; set; } 
    }

    public class ExtratoData
    {
        public string UniqueId { get; set; } 
        public DateTime Date { get; set; } 
        public string Type { get; set; } 
        public decimal Balance { get; set; } 
        public DateTime DateStart { get; set; } 
        public DateTime DateEnd { get; set; } 
    }

    public class Meta
    {
        public int Count { get; set; } 
        public int Page { get; set; } 
        public int TotalPages { get; set; } 
    }

}
