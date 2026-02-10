using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class Blog : BaseModel
    {

        public int IdBlog { get; private set; }
        public int? IdEmpresa { get; private set; }
        public Empresa Empresa { get; private set; }
        public string Autor { get; private set; }
        public string Titulo { get; private set; }
        public string Subtitulo { get; private set; }
        public string Descricao {  get; private set; }
        public DateTime? Data {  get; private set; }
        public string LinkFoto {  get; private set; }


        public Blog() { }

        public Blog(Empresa empresa, string autor, string titulo, string subtitulo, string descricao, DateTime? data, string linkFoto, string usuarioInclusao)
        {
            Empresa = empresa;
            Autor = autor;
            Titulo = titulo;
            Subtitulo = subtitulo;
            Descricao = descricao;
            Data = data;
            LinkFoto = linkFoto;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Empresa empresa, string autor, string titulo, string subtitulo, string descricao, DateTime? data, string linkFoto, string usuarioAlteracao)
        {
            Empresa = empresa;
            Autor = autor;
            Titulo = titulo;
            Subtitulo = subtitulo;
            Descricao = descricao;
            Data = data;
            LinkFoto = linkFoto;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
            Valida();
        }

        public void Valida()
        {
            //if (IdEmpresa == null)
                //throw new Exception("Empresa é obrigatório");
            if (string.IsNullOrEmpty(Autor))
                throw new Exception("Autor é obrigatório");
            if (string.IsNullOrEmpty(Titulo))
                throw new Exception("Autor é obrigatório");
            if (string.IsNullOrEmpty(Subtitulo))
                throw new Exception("Autor é obrigatório");
            if (Data == default(DateTime))
                throw new Exception("A Data incorreta!");
        }
    }
}

