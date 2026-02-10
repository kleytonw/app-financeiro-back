using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class BlogComentario : BaseModel
    {
        public int IdBlogComentario {  get; private set; }
        public int? IdBlog { get; private set; }
        public Blog Blog { get; private set; }
        public string Nome {  get; private set; }
        public string Email {  get; private set; }
        public string Comentario { get; private set; }


        public BlogComentario() { }

        public BlogComentario(Blog blog, string nome, string email, string comentario, string usuarioInclusao)
        {
            Blog = blog;
            Nome = nome;
            Email = email;
            Comentario = comentario;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Blog blog, string nome, string email, string comentario, string usuarioAlteracao)
        {
            Blog = blog;
            Nome = nome;
            Email = email;
            Comentario = comentario;
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
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("Nome é obrigatório");
            if (string.IsNullOrEmpty(Email))
                throw new Exception("Nome é obrigatório");
        }
    }
}


