using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace ERP.Models
{
    public class MovimentacaoLogResponse
    {
        public int IdMovimentacaoLog { get; set; }
        public int IdEmpresa { get; set; }
        public Date DataMovimentacaoLog { get; set; }
        public string Situacao { get; set; }
    }

    public class MovimentacaoLogRequest
    {
        public int IdMovimentacaoLog { get; set; }
        public int IdEmpresa { get; set; }
        public Date DataMovimentacaoLog { get; set; }
        public string Situacao { get; set; }
    }
}
