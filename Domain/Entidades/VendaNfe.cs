using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class VendaNfe : BaseModel
    {
        public int IdVendaNfe { get; set; }
        public Cliente Cliente { get; set; }
        public int IdCliente { get; set; }
        public string Senha { get; set; }
        public DateTime DataVenda { get; set; }
        public int Modelo { get; set; } 
        public string Arquivo { get; set; }


        public VendaNfe() { }

        public VendaNfe(Cliente cliente, string senha, DateTime dataVenda, int modelo, string arquivo, string usuarioInclusao)
        {
            Cliente = cliente;
            Senha = senha;
            DataVenda = dataVenda;
            Modelo = modelo;
            Arquivo = arquivo;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Cliente cliente, string senha, DateTime dataVenda, int modelo, string arquivo, string usuarioAlteracao)
        {
            Cliente = cliente;
            Senha = senha;
            DataVenda = dataVenda;
            Modelo = modelo;
            Arquivo = arquivo;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Cliente == null)
                throw new Exception("Cliente é obrigatório");
            if (string.IsNullOrEmpty(Senha))
                throw new Exception("Senha é obrigatória");
            if (DataVenda == default)
                throw new Exception("Data da venda é obrigatória");

        }
    }
}
