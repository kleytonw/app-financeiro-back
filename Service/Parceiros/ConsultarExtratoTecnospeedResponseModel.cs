using System.Collections.Generic;
using System;

namespace ERP_API.Service.Parceiros
{
    public class ConsultarExtratoTecnospeedResponseModel
    {
        public BankStatement BankStatement { get; set; }
        public Transactions Transactions { get; set; }
    }

    public class BankStatement
    {
        public string BankCode { get; set; }
        public string Bank { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public int TotalTransactions { get; set; }
        public string AccountHash { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }

    public class Transactions
    {
        public List<TransactionDetail> Credit { get; set; }
        public List<TransactionDetail> Debit { get; set; }
        public BalanceDetail Balance { get; set; }

        public class TransactionDetail
        {
            public int Sequence { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string FitId { get; set; }
            public string Name { get; set; }
        }

        public class BalanceDetail
        {
            public BalanceAmount Inicial { get; set; }
            public BalanceAmount Final { get; set; }
        }

        public class BalanceAmount
        {
            public DateTime Date { get; set; }
            public decimal Balance { get; set; }
        }

    }
}
