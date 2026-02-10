using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ClienteDocumento : BaseModel
    {
        public int IdClienteDocumento { get; private set; }
        public int IdCliente { get; private set; }
        public Cliente Cliente { get; private set; }
        public int IdTipoDocumento { get; private set; }
        public TipoDocumento TipoDocumento { get; private set; }
        public string Arquivo {  get; private set; }

        public ClienteDocumento() { }

        public ClienteDocumento(Cliente cliente, TipoDocumento tipoDocumento, string arquivo, string usuarioInclusao)
        {
            Cliente = cliente;
            TipoDocumento = tipoDocumento;
            Arquivo = arquivo;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Cliente cliente, TipoDocumento tipoDocumento, string arquivo, string usuarioAlteracao)
        {
            Cliente = cliente;
            TipoDocumento = tipoDocumento;
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
                throw new Exception("O cliente é obrigatório!");
            if (TipoDocumento == null)
                throw new Exception("O tipo de documento é obrigatório!");
            if (string.IsNullOrEmpty(Arquivo))
                throw new Exception("O arquivo não pode ser vazio.");
        }

    }
}
